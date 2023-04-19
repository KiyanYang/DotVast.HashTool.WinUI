// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashTaskService
{
    ObservableCollection<HashTask> HashTasks { get; }

    /// <summary>
    /// StartupAsync <see cref="IActivationService.ActivateAsync(object)"/>.
    /// </summary>
    /// <returns></returns>
    Task StartupAsync();
}
