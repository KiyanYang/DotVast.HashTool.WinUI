using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
    Task<HashTask> HashFile(HashTask hashTask, IProgress<double> atomProgress, IProgress<double> taskProgress, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashFolder(HashTask hashTask, IProgress<double> atomProgress, IProgress<double> taskProgress, ManualResetEventSlim mres, CancellationToken ct);
    Task<HashTask> HashText(HashTask hashTask, IProgress<double> atomProgress, IProgress<double> taskProgress, ManualResetEventSlim mres, CancellationToken ct);
}
