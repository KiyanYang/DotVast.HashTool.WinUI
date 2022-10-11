using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashTaskService
{
    IList<HashTask> HashTasks { get; }
}
