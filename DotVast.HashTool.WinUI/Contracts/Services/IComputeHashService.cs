using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
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
