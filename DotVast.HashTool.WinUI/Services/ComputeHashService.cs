using System.Diagnostics;
using System.Security.Cryptography;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class ComputeHashService : IComputeHashService
{
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
        for (var i = 0; i < filePaths.Count; i++)
        {
            // 出现异常情况的频率较低，因此不使用 File.Exists 等涉及 IO 的额外判断操作
            try
            {
                //hashTask.ProgressMax = filesCount;
                hashTask.ProgressMax1?.Report(filesCount);
                using var stream = File.Open(filePaths[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                var hashResult = await Task.Run(() => HashStream(hashTask, stream, mres, ct, i));
                if (hashResult != null)
                {
                    hashResult.Type = HashResultType.File;
                    hashResult.Content = filePaths[i];
                    hashTask.Results.Add(hashResult);
                }
            }
            catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
            {
                WeakReferenceMessenger.Default.Send(new FileNotFoundInHashFilesMessage(filePaths[i]));
                filesCount--;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<HashTask> HashTextAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await PreAndPostProcessAsync(async () =>
        {
            //hashTask.ProgressMax = 1;
            hashTask.ProgressMax1?.Report(1);
            var contentBytes = hashTask.Encoding!.GetBytes(hashTask.Content);
            using Stream stream = new MemoryStream(contentBytes);
            var hashResult = await Task.Run(() => HashStream(hashTask, stream, mres, ct, 0));
            if (hashResult != null)
            {
                hashResult.Type = HashResultType.Text;
                hashResult.Content = hashTask.Content;
                hashTask.Results = new() { hashResult };
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

    private HashResult? HashStream(HashTask hashTask, Stream stream, ManualResetEventSlim mres, CancellationToken ct, double progressOffset)
    {
        HashAlgorithm[] hashes = hashTask.SelectedHashs.Select(x => Hash.GetHashAlgorithm(x)!).ToArray();

        #region 初始化, 定义文件流读取参数及变量.

        stream.Position = 0;

        // 设定缓冲区大小，分为 4 档：
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
        // 每次读取长度。此初值仅用于开启初次计算，真正的首次赋值在屏障内完成。
        var readLength = bufferSize;

        #endregion

        #region 使用屏障并行计算哈希值

        ThreadPool.GetMinThreads(out var minWorker, out var minIOC);
        ThreadPool.SetMinThreads(hashes.Length, minIOC);

        using Barrier barrier = new(hashes.Length, (b) =>
        {
            if (ct.IsCancellationRequested)
            {
                readLength = 0;
                return;
            }

            // 实际读取长度. 读取完毕时该值为 0.
            readLength = stream.Read(buffer, 0, bufferSize);

            if (!mres.IsSet)
            {
                Status = ComputeHashStatus.Pasue;
                mres.Wait();
                Status = ComputeHashStatus.Busy;
            }

            // 报告进度. streamLength 在此处始终大于 0.
            //hashTask.ProgressVal = (double)stream.Position / stream.Length + progressOffset;
            hashTask.ProgressVal1?.Report((double)stream.Position / stream.Length + progressOffset);
        });

        // 定义本地函数。当读取长度大于 0 时，先屏障同步（包括读取文件、报告进度等），再并行计算。
        void Action(HashAlgorithm hash)
        {
            while (readLength > 0)
            {
                barrier.SignalAndWait(CancellationToken.None);
                hash.TransformBlock(buffer, 0, readLength, null, 0);
#if DEBUG // 睡眠一段时间，以便观察进度条。
                Thread.Sleep(50);
#endif
            }
        }

        Parallel.ForEach(hashes, Action);
        ThreadPool.SetMinThreads(minWorker, minIOC);

        #endregion

        if (ct.IsCancellationRequested)
        {
            //hashTask.ProgressVal = progressOffset;
            hashTask.ProgressVal1?.Report(progressOffset);
            return null;
        }
        else
        {
            // 确保报告计算完成. 主要用于解决空流(stream.Length == 0)时无法在屏障进行报告的问题.
            //hashTask.ProgressVal = progressOffset + 1;
            hashTask.ProgressVal1?.Report(progressOffset + 1);
        }

        var hashResultData = new HashResultItem[hashes.Length];

        for (int i = 0; i < hashes.Length; i++)
        {
            // 处理最后数据块
            hashes[i].TransformFinalBlock(buffer, 0, 0);

            // 设置结果
            var hashBytes = hashes[i].Hash!;
            var hashString = hashTask.SelectedHashs[i] == Hash.QuickXor
                ? Convert.ToBase64String(hashBytes)
                : Convert.ToHexString(hashBytes);
            hashResultData[i] = new(hashTask.SelectedHashs[i], hashString);

            // 释放非托管资源
            hashes[i].Dispose();
        }

        return new HashResult() { Data = hashResultData };
    }

    private void OnStatusChanged(ComputeHashStatus value)
    {
        var statusChanged = StatusChanged;
        statusChanged?.Invoke(this, value);
    }
}
