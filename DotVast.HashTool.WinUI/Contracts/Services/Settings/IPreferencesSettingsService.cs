using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IPreferencesSettingsService : IBaseObservableSettings
{
    ObservableCollection<HashOption> HashOptions { get; }

    /// <summary>
    /// 检查更新时是否包括预发行版本.
    /// </summary>
    bool IncludePreRelease { get; set; }

    Task SaveHashOptionsAsync();
}
