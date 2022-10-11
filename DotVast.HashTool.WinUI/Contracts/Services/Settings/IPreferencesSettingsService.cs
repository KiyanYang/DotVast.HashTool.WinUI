using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IPreferencesSettingsService : IBaseObservableSettings
{
    ObservableCollection<HashOption> HashOptions { get; }

    Task SaveHashOptionsAsync();
}
