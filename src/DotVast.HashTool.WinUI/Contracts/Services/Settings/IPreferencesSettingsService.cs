using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IPreferencesSettingsService : IBaseObservableSettings
{
    IReadOnlyList<HashSetting> HashSettings { get; }

    /// <summary>
    /// 保存 <see cref="HashSetting"/>
    /// </summary>
    /// <param name="hashSetting">要保存的哈希设置.</param>
    /// <param name="forContextMenu">是否包含对资源管理器上下文菜单的修改.</param>
    void SaveHashSetting(HashSetting hashSetting, bool forContextMenu = false);

    /// <summary>
    /// 要跳过的文件的属性.
    /// </summary>
    FileAttributes FileAttributesToSkip { get; set; }

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
