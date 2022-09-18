using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
    /// <summary>
    /// 当前流计算的进度.
    /// </summary>
    Progress<double> AtomProgress
    {
        get;
    }

    /// <summary>
    /// 当前任务的进度. (Val 当前已计算的数量, Max 总量)
    /// </summary>
    Progress<(int Val, int Max)> TaskProgress
    {
        get;
    }

    /// <summary>
    /// 是否空闲.
    /// </summary>
    bool IsFree
    {
        get;
    }

    Task<HashTask> HashFile(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashFolder(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashText(HashTask hashTask, ManualResetEventSlim mres, CancellationToken ct);
}
