using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class HashTaskService : IHashTaskService
{
    public ObservableCollection<HashTask> HashTasks { get; } = new();

    public Task StartupAsync()
    {
        foreach (var item in HashTasks.Where(x => x.State == HashTaskState.Waiting))
        {
            _ = item.StartAsync();
        }
        return Task.CompletedTask;
    }
}
