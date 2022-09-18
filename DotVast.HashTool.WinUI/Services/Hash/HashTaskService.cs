using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services.Hash;

internal class HashTaskService : IHashTaskService
{
    public IList<HashTask> HashTasks { get; } = new ObservableCollection<HashTask>();
}
