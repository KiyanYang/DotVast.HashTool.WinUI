using System.Diagnostics;
using System.Security.Cryptography;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

using Microsoft.UI.Dispatching;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class ComputeHashService : IComputeHashService
{
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
                hashTask.ProgressMax = filesCount;
                using var stream = File.Open(filePaths[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                var hashResult = await Task.Run(() => HashStream(hashTask, stream, i, mres, ct));
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
            ArgumentNullException.ThrowIfNull(hashTask.Encoding);

            hashTask.ProgressMax = 1;
            var contentBytes = hashTask.Encoding.GetBytes(hashTask.Content);
            using Stream stream = new MemoryStream(contentBytes);
            var hashResult = await Task.Run(() => HashStream(hashTask, stream, 0, mres, ct));
            if (hashResult != null)
            {
                hashResult.Type = HashResultType.Text;
                hashResult.Content = hashTask.Content;
                hashTask.Results = new() { hashResult };
            }
        }, hashTask, ct);
    }

    private static async Task<HashTask> PreAndPostProcessAsync(Func<Task> func, HashTask hashTask, CancellationToken ct)
    {
        var stopWatch = Stopwatch.StartNew();
        hashTask.State = HashTaskState.Working;
        hashTask.Elapsed = default;
        hashTask.Results = default;

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
        }
    }

    /// <summary>
    /// 计算流的哈希值.
    /// </summary>
    /// <param name="hashTask">哈希任务.</param>
    /// <param name="stream">要计算的流.</param>
    /// <param name="progressOffset">进度偏移量, 用于多文件等模式, 该偏移量等于已计算的数量.</param>
    /// <param name="mres">控制暂停.</param>
    /// <param name="ct">控制取消.</param>
    /// <returns>哈希结果.</returns>
    private HashResult? HashStream(HashTask hashTask, Stream stream, double progressOffset, ManualResetEventSlim mres, CancellationToken ct)
    {
        var hashes = hashTask.SelectedHashs.Select(Hash.GetHashAlgorithm).OfType<HashAlgorithm>().ToArray();

        try
        {
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
                if (!mres.IsSet)
                {
                    TryEnqueue(() => hashTask.State = HashTaskState.Paused);
                    mres.Wait();
                    // 在下方 State 刷新前, 进行一次判断, 以避免 UI 状态的错误改变.
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    TryEnqueue(() => hashTask.State = HashTaskState.Working);
                }

                // 实际读取长度. 读取完毕时该值为 0.
                readLength = stream.Read(buffer, 0, bufferSize);

                // 报告进度. stream.Length 在此处始终大于 0.
                TryEnqueue(() => hashTask.ProgressVal = (double)stream.Position / stream.Length + progressOffset);
            });

            // 定义本地函数。当读取长度大于 0 时，先屏障同步（包括读取文件、报告进度等），再并行计算。
            void Action(HashAlgorithm hash)
            {
                while (readLength > 0)
                {
                    barrier.SignalAndWait(ct);
                    hash.TransformBlock(buffer, 0, readLength, null, 0);
#if DEBUG // 睡眠一段时间，以便观察进度条。
                    Thread.Sleep(50);
#endif
                }
            }

            Parallel.ForEach(hashes, new() { CancellationToken = ct }, Action);
            ThreadPool.SetMinThreads(minWorker, minIOC);

            #endregion

            // 确保报告计算完成. 主要用于解决空流(stream.Length == 0)时无法在屏障进行报告的问题.
            TryEnqueue(() => hashTask.ProgressVal = progressOffset + 1);

            var hashResultData = new HashResultItem[hashes.Length];

            for (int i = 0; i < hashes.Length; i++)
            {
                hashes[i].TransformFinalBlock(buffer, 0, 0);

                // 设置结果
                var hashBytes = hashes[i].Hash!;
                var hashString = hashTask.SelectedHashs[i] == Hash.QuickXor
                    ? Convert.ToBase64String(hashBytes)
                    : Convert.ToHexString(hashBytes);
                hashResultData[i] = new(hashTask.SelectedHashs[i], hashString);
            }

            return new HashResult() { Data = hashResultData };
        }
        finally
        {
            foreach (var hash in hashes)
            {
                hash.Dispose();
            }
        }
    }

    private static void TryEnqueue(DispatcherQueueHandler handler)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(handler);
    }
}
