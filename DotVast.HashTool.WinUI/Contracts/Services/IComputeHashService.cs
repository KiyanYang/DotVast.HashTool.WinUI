using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
    /// <summary>
    /// 当前流计算的进度.
    /// </summary>
    event EventHandler<double> AtomProgressChanged;

    /// <summary>
    /// 当前任务的进度. (Val 当前已计算的数量, Max 总量)
    /// </summary>
    event EventHandler<(int Val, int Max)> TaskProgressChanged;

    /// <summary>
    /// 当前服务的状态.
    /// </summary>
    ComputeHashStatus Status
    {
        get;
    }

    Task<HashTask> HashFile(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashFolder(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashText(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
}

public enum ComputeHashStatus
{
    Free,
    Busy,
    Pasue,
}
