using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
    /// <summary>
    /// 当计算的进度改变时触发.
    /// </summary>
    event EventHandler<double> AtomProgressChanged;

    /// <summary>
    /// 当任务的进度改变时触发.
    /// </summary>
    /// <remarks>
    /// Val: 已计算的数量. Max: 任务总量.
    /// </remarks>
    event EventHandler<(int Val, int Max)> TaskProgressChanged;

    /// <summary>
    /// 当该服务状态改变时触发.
    /// </summary>
    event EventHandler<ComputeHashStatus> StatusChanged;

    /// <summary>
    /// 当前服务的状态.
    /// </summary>
    ComputeHashStatus Status { get; }

    Task<HashTask> HashFileAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashFolderAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashTextAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
}
