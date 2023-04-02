using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IPreferencesSettingsService : IBaseObservableSettings
{
    ObservableCollection<HashSetting> HashSettings { get; }

    Task SaveHashSettingsAsync();

    /// <summary>
    /// 是否启用资源管理器注册的上下文菜单.
    /// </summary>
    bool FileExplorerContextMenusEnabled { get; set; }

    /// <summary>
    /// 检查更新时是否包括预发行版本.
    /// </summary>
    bool IncludePreRelease { get; set; }

    /// <summary>
    /// 是否启动时检查更新.
    /// </summary>
    bool CheckForUpdatesOnStartup { get; set; }

    /// <summary>
    /// 创建任务时是否开始计算.
    /// </summary>
    bool StartingWhenCreateHashTask { get; set; }
}
