using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashTaskService
{
    ObservableCollection<HashTask> HashTasks { get; }
}
