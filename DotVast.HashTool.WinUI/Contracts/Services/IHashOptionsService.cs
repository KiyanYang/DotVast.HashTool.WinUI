using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashOptionsService
{
    ObservableCollection<HashOption> HashOptions
    {
        get;
    }

    Task InitializeAsync();

    Task SetHashOptionsAsync(IList<HashOption> hashOptions);
}
