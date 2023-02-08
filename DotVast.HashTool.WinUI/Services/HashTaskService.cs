using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal class HashTaskService : IHashTaskService
{
    public ObservableCollection<HashTask> HashTasks { get; } = new();

    public async Task StartupAsync()
    {
        foreach (var item in HashTasks.Where(x => x.State == HashTaskState.Waiting))
        {
            await item.StartAsync();
        }
    }
}
