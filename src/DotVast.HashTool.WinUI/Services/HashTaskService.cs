// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class HashTaskService : IHashTaskService
{
    public ObservableCollection<HashTask> HashTasks { get; } = [];

    public Task StartupAsync()
    {
        foreach (var item in HashTasks.Where(x => x.State == HashTaskState.Waiting))
        {
            _ = item.StartAsync();
        }
        return Task.CompletedTask;
    }
}
