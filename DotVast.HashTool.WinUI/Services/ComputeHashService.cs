using System.Buffers;
using System.Diagnostics;
using System.Security.Cryptography;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class ComputeHashService : IComputeHashService
{
    private const int BufferSize = 1024 * 1024 * 4;

    public async Task ComputeHashAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        hashTask.State = HashTaskState.Working;
        hashTask.Elapsed = default;
        hashTask.Results = default;

        try
        {
            switch (hashTask.Mode)
            {
                case var m when m == HashTaskMode.File:
                    await InternalHashFilesAsync(hashTask, hashTask.Content.Split('|'), mres, cancellationToken);
                    break;
                case var m when m == HashTaskMode.Folder:
                    await InternalHashFilesAsync(hashTask, Directory.GetFiles(hashTask.Content), mres, cancellationToken);
                    break;
                case var m when m == HashTaskMode.Text:
                    await InternalHashTextAsync(hashTask, mres, cancellationToken);
                    break;
            }
            hashTask.State = HashTaskState.Completed;
        }
        catch (AggregateException ex)
        {
            if (ex.InnerExceptions.All(e => e is OperationCanceledException))
            {
                App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Canceled);
                throw new OperationCanceledException(cancellationToken);
            }
            else
            {
                App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Aborted);
                throw;
            }
        }
        catch (OperationCanceledException)
        {
            App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Canceled);
            throw;
        }
        catch (Exception)
        {
            App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Aborted);
            throw;
        }
        finally
        {
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
            App.MainWindow.TryEnqueue(() => hashTask.Elapsed = elapsed);
        }
    }

    private async Task InternalHashFilesAsync(HashTask hashTask, IList<string> filePaths, ManualResetEventSlim mres, CancellationToken cancellationToken)
    {
        hashTask.Results = new();
        var failedFilesCount = 0;
        var filesCount = filePaths.Count;
        for (var i = 0; i < filePaths.Count; i++)
        {
            // 出现异常情况的频率较低，因此不使用 File.Exists 等涉及 IO 的额外判断操作
            try
            {
                hashTask.ProgressMax = filesCount - failedFilesCount;
                using var stream = File.Open(filePaths[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                var hashResult = await Task.Run(() => HashStream(hashTask, stream, i - failedFilesCount, mres, cancellationToken));
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
                failedFilesCount++;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private async Task InternalHashTextAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken CancellationToken)
    {
        ArgumentNullException.ThrowIfNull(hashTask.Encoding);

        hashTask.ProgressMax = 1;
        var contentBytes = hashTask.Encoding.GetBytes(hashTask.Content);
        using Stream stream = new MemoryStream(contentBytes);
        var hashResult = await Task.Run(() => HashStream(hashTask, stream, 0, mres, CancellationToken));
        if (hashResult != null)
        {
            hashResult.Type = HashResultType.Text;
            hashResult.Content = hashTask.Content;
            hashTask.Results = new() { hashResult };
        }
    }

    /// <summary>
    /// 计算流的哈希值.
    /// </summary>
    /// <param name="hashTask">哈希任务.</param>
    /// <param name="stream">要计算的流.</param>
    /// <param name="progressOffset">进度偏移量, 用于多文件等模式, 该偏移量等于已计算的数量.</param>
    /// <param name="mres">控制暂停.</param>
    /// <param name="cancellationToken">控制取消.</param>
    /// <returns>哈希结果.</returns>
    private HashResult? HashStream(HashTask hashTask,
        Stream stream,
        double progressOffset,
        ManualResetEventSlim mres,
        CancellationToken cancellationToken)
    {
        if (hashTask.SelectedHashs.Count == 1)
        {
            return HashStreamForSingleHashAlgorithm(hashTask, stream, progressOffset, mres, cancellationToken);
        }
        else
        {
            return HashStreamForMultiHashAlgorithm(hashTask, stream, progressOffset, mres, cancellationToken);
        }
    }

    private HashResult? HashStreamForMultiHashAlgorithm(HashTask hashTask,
        Stream stream,
        double progressOffset,
        ManualResetEventSlim mres,
        CancellationToken cancellationToken)
    {
        var hashAlgorithms = hashTask.SelectedHashs.Select(Hash.GetHashAlgorithm).OfType<HashAlgorithm>().ToArray();
        Debug.Assert(hashTask.SelectedHashs.Count == hashAlgorithms.Length);

        byte[]? buffer = null;
        int clearLimit = 0;

        try
        {
            stream.Seek(0, SeekOrigin.Begin);

            buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            // 每次读取长度。当 stream.Length == 0 时，readLength 为 0，此时不会进入 barrier.postPhaseAction，即保证其内 stream.Length > 0。
            int readLength = Math.Sign(stream.Length);

            #region 使用屏障并行计算哈希值

            ThreadPool.GetMinThreads(out var minWorker, out var minIOC);
            ThreadPool.SetMinThreads(hashAlgorithms.Length, minIOC);

            using Barrier barrier = new(hashAlgorithms.Length, (b) =>
            {
                if (!mres.IsSet)
                {
                    App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Paused);
                    mres.Wait(CancellationToken.None);
                    // 在下方 State 刷新前, 进行一次判断, 以避免 UI 状态的错误改变.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Working);
                }

                // 实际读取长度. 读取完毕时该值为 0.
                readLength = stream.Read(buffer, 0, BufferSize);
                clearLimit = Math.Max(clearLimit, readLength);

                // 报告进度. stream.Length 在此处始终大于 0.
                App.MainWindow.TryEnqueue(() => hashTask.ProgressVal = (double)stream.Position / stream.Length + progressOffset);
            });

            // 定义本地函数。当读取长度大于 0 时，先屏障同步（包括读取文件、报告进度等），再并行计算。
            void Action(HashAlgorithm hashAlgorithm)
            {
                while (readLength > 0)
                {
                    barrier.SignalAndWait(cancellationToken);
                    hashAlgorithm.TransformBlock(buffer, 0, readLength, null, 0);
#if DOTVAST_SLOWCPU // 睡眠一段时间，以便观察进度条。
                    Thread.Sleep(50);
#endif
                }
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            }

            Parallel.ForEach(hashAlgorithms, Action);
            ThreadPool.SetMinThreads(minWorker, minIOC);

            // 确保报告计算完成. 主要用于当 stream.Length == 0 时没有在 barrier.postPhaseAction 进行报告的情况.
            App.MainWindow.TryEnqueue(() => hashTask.ProgressVal = progressOffset + 1);

            #endregion

            var hashResultData = new HashResultItem[hashAlgorithms.Length];

            for (int i = 0; i < hashAlgorithms.Length; i++)
            {
                var hashString = GetFormattedHash(hashAlgorithms[i].Hash!, hashTask.SelectedHashs[i].Format);
                hashResultData[i] = new(hashTask.SelectedHashs[i], hashString);
            }

            return new HashResult() { Data = hashResultData };
        }
        finally
        {
            foreach (var hash in hashAlgorithms)
            {
                hash.Dispose();
            }
            if (buffer is not null)
            {
                CryptographicOperations.ZeroMemory(buffer.AsSpan(0, clearLimit));
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    private HashResult? HashStreamForSingleHashAlgorithm(
        HashTask hashTask,
        Stream stream,
        double progressOffset,
        ManualResetEventSlim mres,
        CancellationToken cancellationToken)
    {
        var hash = hashTask.SelectedHashs[0];
        var hashAlgorithm = hash.GetHashAlgorithm();

        byte[]? buffer = null;
        int clearLimit = 0;

        try
        {
            stream.Seek(0, SeekOrigin.Begin);

            buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            int readLength = 0;

            while ((readLength = stream.Read(buffer, 0, BufferSize)) > 0)
            {
                clearLimit = Math.Max(clearLimit, readLength);

                if (!mres.IsSet)
                {
                    App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Paused);
                    mres.Wait(CancellationToken.None);
                    // 在下方 State 刷新前, 进行一次判断, 以避免 UI 状态的错误改变.
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        App.MainWindow.TryEnqueue(() => hashTask.State = HashTaskState.Working);
                    }
                }
                cancellationToken.ThrowIfCancellationRequested();

                // 报告进度. stream.Length 在此处始终大于 0.
                App.MainWindow.TryEnqueue(() => hashTask.ProgressVal = (double)stream.Position / stream.Length + progressOffset);

                hashAlgorithm.TransformBlock(buffer, 0, readLength, null, 0);
#if DOTVAST_SLOWCPU // 睡眠一段时间，以便观察进度条。
                Thread.Sleep(50);
#endif
            }
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);

            // 确保报告计算完成. 主要用于当 stream.Length == 0 时没有进行过报告的情况.
            App.MainWindow.TryEnqueue(() => hashTask.ProgressVal = progressOffset + 1);

            var hashString = GetFormattedHash(hashAlgorithm.Hash!, hash.Format);
            var hashResultItem = new HashResultItem(hash, hashString);

            return new HashResult() { Data = new[] { hashResultItem } };
        }
        finally
        {
            hashAlgorithm.Dispose();
            if (buffer is not null)
            {
                CryptographicOperations.ZeroMemory(buffer.AsSpan(0, clearLimit));
                ArrayPool<byte>.Shared.Return(buffer, clearArray: false);
            }
        }
    }

    private string GetFormattedHash(byte[] hashData, HashFormat format)
    {
        return format switch
        {
            HashFormat.Base16 => Convert.ToHexString(hashData),
            HashFormat.Base64 => Convert.ToBase64String(hashData),
            _ => throw new ArgumentOutOfRangeException(nameof(format), $"The HashFormat {format} is out of range and cannot be processed."),
        };
    }
}
