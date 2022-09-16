using System.Diagnostics;
using System.Text;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services.Hash;

internal class ComputeHashService : IComputeHashService
{
    private readonly IProgress<double> _atomProgress = new Progress<double>();
    public Progress<double> AtomProgress => (Progress<double>)_atomProgress;

    private readonly IProgress<(int Val, int Max)> _taskProgress = new Progress<(int, int)>();
    public Progress<(int Val, int Max)> TaskProgress => (Progress<(int, int)>)_taskProgress;

    public bool IsFree
    {
        get; set;
    } = true;

    public async Task<HashTask> HashFile(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await AttachHashTask(async () =>
        {
            _taskProgress.Report((0, 1));
            using Stream stream = File.Open(hashTask.Content, FileMode.Open, FileAccess.Read, FileShare.Read);
            var hashResult = await Task.Run(() => HashStream(hashTask.SelectedHashs, stream, _atomProgress, mres, ct));
            if (hashResult != null)
            {
                hashResult.Type = HashResultType.File;
                hashResult.Content = hashTask.Content;
                hashTask.Result = hashResult;
                _taskProgress.Report((1, 1));
            }
            return;
        }, hashTask, ct);
    }

    public async Task<HashTask> HashFolder(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await AttachHashTask(async () =>
        {
            hashTask.Results = new();
            var filePaths = Directory.GetFiles(hashTask.Content);
            _taskProgress.Report((0, filePaths.Length));
            foreach (var filePath in filePaths)
            {
                using Stream? stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (stream == null)
                {
                    continue;
                }

                var hashResult = await Task.Run(() => HashStream(hashTask.SelectedHashs, stream, _atomProgress, mres, ct));
                if (hashResult != null)
                {
                    hashResult.Type = HashResultType.File;
                    hashResult.Content = filePath;
                    hashTask.Results.Add(hashResult);
                }
                _taskProgress.Report((hashTask.Results.Count, filePaths.Length));
            }
        }, hashTask, ct);
    }

    public async Task<HashTask> HashText(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct)
    {
        return await AttachHashTask(async () =>
        {
            _taskProgress.Report((0, 1));
            var contentBytes = hashTask.Encoding?.GetBytes(hashTask.Content) ?? Array.Empty<byte>();
            using Stream stream = new MemoryStream(contentBytes);
            var hashResult = await Task.Run(() => HashStream(hashTask.SelectedHashs, stream, _atomProgress, mres, ct));
            if (hashResult != null)
            {
                hashResult.Type = HashResultType.Text;
                hashResult.Content = hashTask.Content;
                hashTask.Result = hashResult;
                _taskProgress.Report((1, 1));
            }
            return;
        }, hashTask, ct);
    }

    public async Task<HashTask> AttachHashTask(Func<Task> func, HashTask hashTask, CancellationToken ct)
    {
        IsFree = false;
        var stopWatch = Stopwatch.StartNew();

        hashTask.Status = HashTaskState.Working;
        await func();
        if (ct.IsCancellationRequested)
        {
            _taskProgress.Report((0, 1));
            hashTask.Status = HashTaskState.Canceled;
        }
        else
        {
            hashTask.Status = HashTaskState.Completed;
        }

        stopWatch.Stop();
        hashTask.Elapsed = stopWatch.Elapsed;
        IsFree = true;
        return hashTask;
    }

    public static HashResult? HashStream(IList<Hash> hashs, Stream stream, IProgress<double> progress, ManualResetEventSlim mres, CancellationToken ct)
    {
        #region 初始化, 定义文件流读取参数及变量.

        var streamLength = stream.Length;
        stream.Position = 0;

        // 设定缓冲区大小，分为 4 档：
        // Length <  32 KiB => Length
        // Length <   4 MiB => 32 KiB
        // Length < 256 MiB =>  1 MiB
        // _                =>  4 MiB
        var bufferSize = streamLength switch
        {
            < 1024 * 32 => (int)streamLength,
            < 1024 * 1024 * 4 => 1024 * 32,
            < 1024 * 1024 * 256 => 1024 * 1024,
            _ => 1024 * 1024 * 4,
        };
        var buffer = new byte[bufferSize];
        // 每次读取长度。此初值仅用于开启初次计算，真正的首次赋值在屏障内完成。
        var readLength = bufferSize;

        #endregion

        #region 使用屏障并行计算哈希值

        using Barrier barrier = new(hashs.Count, (b) =>
        {
            // 实际读取长度. 读取完毕时该值为 0.
            readLength = stream.Read(buffer, 0, bufferSize);

            // 暂停功能.
            mres.Wait();

            // 报告进度. streamLength 在此处始终大于 0.
            progress.Report((double)stream.Position / streamLength);
        });

        // 定义本地函数。当读取长度大于 0 时，先屏障同步（包括读取文件、报告进度等），再并行计算。
        void action(Hash hash)
        {
            while (readLength > 0)
            {
                if (ct.IsCancellationRequested)
                {
                    barrier.RemoveParticipant();
                    break;
                }
                barrier.SignalAndWait(CancellationToken.None);
                hash.Algorithm.TransformBlock(buffer, 0, readLength, null, 0);
            }
        }

        // 开启并行动作
        Parallel.ForEach(hashs, action);

        #endregion

        if (ct.IsCancellationRequested)
        {
            progress.Report(0);
            return null;
        }
        else
        {
            // 确保报告计算完成. 主要用于解决空流(stream.Length == 0)时无法在屏障进行报告的问题.
            progress.Report(1);
        }

        HashResult hashResult = new()
        {
            Data = new List<HashResultItem>()
        };
        foreach (var item in hashs)
        {
            item.Algorithm.TransformFinalBlock(buffer, 0, 0);
            if (item.Algorithm.Hash is byte[] hashValue)
            {
                hashResult.Data.Add(MakeHashResultItem(item, item.Algorithm.Hash));
            }
        }

        return hashResult;
    }

    private static HashResultItem MakeHashResultItem(Hash hash, byte[] data)
    {
        var val = hash == Hash.QuickXor
            ? HashFormatBase64(data)
            : HashFormatHex(data);
        return new HashResultItem(hash, val);
    }

    #region 格式化哈希值 bytes => string

    private static string HashFormatHex(byte[] data)
    {
        StringBuilder sBuilder = new(data.Length << 1);

        foreach (var b in data)
        {
            sBuilder.Append($"{b:X2}");
        }

        return sBuilder.ToString();
    }

    private static string HashFormatBase64(byte[] data)
    {
        return Convert.ToBase64String(data);
    }

    #endregion 格式化哈希值 bytes => string
}
