using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class ComputeHashService : IComputeHashService
{
    private readonly IProgress<double> _atomProgress = new Progress<double>();

    private readonly IProgress<(int Val, int Max)> _taskProgress = new Progress<(int, int)>();

    public event EventHandler<double> AtomProgressChanged
    {
        add => (_atomProgress as Progress<double>)!.ProgressChanged += value;
        remove => (_atomProgress as Progress<double>)!.ProgressChanged -= value;
    }

    public event EventHandler<(int Val, int Max)> TaskProgressChanged
    {
        add => (_taskProgress as Progress<(int, int)>)!.ProgressChanged += value;
        remove => (_taskProgress as Progress<(int, int)>)!.ProgressChanged -= value;
    }

    public event EventHandler<ComputeHashStatus>? StatusChanged;

    private ComputeHashStatus _status = ComputeHashStatus.Free;

    public ComputeHashStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnStatusChanged(value);
            }
        }
    }

    public async Task<HashTask> HashFileAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await PreAndPostProcessAsync(async () =>
        {
            await InternalHashFilesAsync(hashTask, hashTask.Content.Split('|'), mres, ct);
        }, hashTask, ct);
    }

    public async Task<HashTask> HashFolderAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await PreAndPostProcessAsync(async () =>
        {
            await InternalHashFilesAsync(hashTask, Directory.GetFiles(hashTask.Content), mres, ct);
        }, hashTask, ct);
    }

    private async Task InternalHashFilesAsync(HashTask hashTask, IList<string> filePaths, ManualResetEventSlim mres, CancellationToken ct)
    {
        hashTask.Results = new();
        var filesCount = filePaths.Count;
        _taskProgress.Report((0, filesCount));
        foreach (var filePath in filePaths)
        {
            // ??????????????????????????????????????????????????? File.Exists ????????? IO ?????????????????????
            try
            {
                using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var hashResult = await Task.Run(() => HashStream(hashTask.SelectedHashs, stream, mres, ct));
                if (hashResult != null)
                {
                    hashResult.Type = HashResultType.File;
                    hashResult.Content = filePath;
                    hashTask.Results.Add(hashResult);
                }
            }
            catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
            {
                WeakReferenceMessenger.Default.Send(new FileNotFoundInHashFilesMessage(filePath));
                filesCount--;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _taskProgress.Report((hashTask.Results.Count, filesCount));
            }
        }
    }

    public async Task<HashTask> HashTextAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await PreAndPostProcessAsync(async () =>
        {
            _taskProgress.Report((0, 1));
            var contentBytes = hashTask.Encoding!.GetBytes(hashTask.Content);
            using Stream stream = new MemoryStream(contentBytes);
            var hashResult = await Task.Run(() => HashStream(hashTask.SelectedHashs, stream, mres, ct));
            if (hashResult != null)
            {
                hashResult.Type = HashResultType.Text;
                hashResult.Content = hashTask.Content;
                hashTask.Results = new() { hashResult };
                _taskProgress.Report((1, 1));
            }
        }, hashTask, ct);
    }

    private async Task<HashTask> PreAndPostProcessAsync(Func<Task> func, HashTask hashTask, CancellationToken ct)
    {
        Status = ComputeHashStatus.Busy;
        var stopWatch = Stopwatch.StartNew();
        hashTask.State = HashTaskState.Working;

        try
        {
            await func();
            if (ct.IsCancellationRequested)
            {
                _taskProgress.Report((0, 1));
                hashTask.State = HashTaskState.Canceled;
            }
            else
            {
                hashTask.State = HashTaskState.Completed;
            }
            return hashTask;
        }
        catch (Exception)
        {
            hashTask.State = HashTaskState.Aborted;
            throw;
        }
        finally
        {
            stopWatch.Stop();
            hashTask.Elapsed = stopWatch.Elapsed;
            Status = ComputeHashStatus.Free;
        }
    }

    private HashResult? HashStream(IList<Hash> hashs, Stream stream, ManualResetEventSlim mres, CancellationToken ct)
    {
        #region ?????????, ????????????????????????????????????.

        stream.Position = 0;

        // ?????????????????????????????? 4 ??????
        // Length <  32 KiB => Length
        // Length <   4 MiB => 32 KiB
        // Length < 512 MiB =>  1 MiB
        // _                =>  4 MiB
        var bufferSize = stream.Length switch
        {
            < 1024 * 32 => (int)stream.Length,
            < 1024 * 1024 * 4 => 1024 * 32,
            < 1024 * 1024 * 512 => 1024 * 1024,
            _ => 1024 * 1024 * 4,
        };
        var buffer = new byte[bufferSize];
        // ??????????????????????????????????????????????????????????????????????????????????????????????????????
        var readLength = bufferSize;

        #endregion

        #region ?????????????????????????????????

        ThreadPool.GetMinThreads(out var minWorker, out var minIOC);
        ThreadPool.SetMinThreads(hashs.Count, minIOC);

        using Barrier barrier = new(hashs.Count, (b) =>
        {
            if (ct.IsCancellationRequested)
            {
                readLength = 0;
                return;
            }

            // ??????????????????. ???????????????????????? 0.
            readLength = stream.Read(buffer, 0, bufferSize);

            if (!mres.IsSet)
            {
                Status = ComputeHashStatus.Pasue;
                mres.Wait();
                Status = ComputeHashStatus.Busy;
            }

            // ????????????. streamLength ????????????????????? 0.
            _atomProgress.Report((double)stream.Position / stream.Length);
        });

        // ?????????????????????????????????????????? 0 ????????????????????????????????????????????????????????????????????????????????????
        void Action(Hash hash)
        {
            while (readLength > 0)
            {
                barrier.SignalAndWait(CancellationToken.None);
                hash.Algorithm.TransformBlock(buffer, 0, readLength, null, 0);
            }
        }

        Parallel.ForEach(hashs, Action);
        ThreadPool.SetMinThreads(minWorker, minIOC);

        #endregion

        if (ct.IsCancellationRequested)
        {
            _atomProgress.Report(0);
            return null;
        }
        else
        {
            // ????????????????????????. ????????????????????????(stream.Length == 0)???????????????????????????????????????.
            _atomProgress.Report(1);
        }

        foreach (var item in hashs)
        {
            item.Algorithm.TransformFinalBlock(buffer, 0, 0);
        }

        return new HashResult()
        {
            Data = hashs.Select(h => MakeHashResultItem(h, h.Algorithm.Hash!)).ToArray(),
        };
    }

    private static HashResultItem MakeHashResultItem(Hash hash, byte[] data)
    {
        var val = hash == Hash.QuickXor
            ? Convert.ToBase64String(data)
            : Convert.ToHexString(data);
        return new HashResultItem(hash.Name, val);
    }

    private void OnStatusChanged(ComputeHashStatus value)
    {
        var statusChanged = StatusChanged;
        statusChanged?.Invoke(this, value);
    }
}
