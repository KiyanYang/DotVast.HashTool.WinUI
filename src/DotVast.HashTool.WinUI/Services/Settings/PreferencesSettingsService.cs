using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;

using static DotVast.HashTool.WinUI.Constants;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class PreferencesSettingsService : BaseObservableSettings, IPreferencesSettingsService
{
    private readonly IHashService _hashService;

    public PreferencesSettingsService(IHashService hashService)
    {
        _hashService = hashService;
    }

    public override async Task InitializeAsync()
    {
        await InitializeHashSettings();
        _fileExplorerContextMenusEnabled = await LoadAsync(nameof(FileExplorerContextMenusEnabled), DefaultPreferencesSettings.FileExplorerContextMenusEnabled);
        _includePreRelease = await LoadAsync(nameof(IncludePreRelease), DefaultPreferencesSettings.IncludePreRelease);
        _checkForUpdatesOnStartup = await LoadAsync(nameof(CheckForUpdatesOnStartup), DefaultPreferencesSettings.CheckForUpdatesOnStartup);
        _startingWhenCreateHashTask = await LoadAsync(nameof(StartingWhenCreateHashTask), DefaultPreferencesSettings.StartingWhenCreateHashTask);
    }

    public override async Task StartupAsync()
    {
        await Task.CompletedTask;
    }

    #region HashSettings
    public ObservableCollection<HashSetting> HashSettings { get; } = new();

    private async Task InitializeHashSettings()
    {
        // TODO: 反序列化时, HashSetting 的属性 Hash 可能为 null
        var hashSettingsFromLocalSettings = await LoadAsync(nameof(HashSettings), Array.Empty<HashSetting>());
        var hashSettings = _hashService.FillHashSettings(hashSettingsFromLocalSettings);
        foreach (var hashSetting in hashSettings)
        {
            HashSettings.Add(hashSetting);
        }
    }

    public async Task SaveHashSettingsAsync()
    {
        await SaveAsync(HashSettings, nameof(HashSettings));
    }
    #endregion HashSettings

    #region FileExplorerContextMenusEnabled
    private bool _fileExplorerContextMenusEnabled;
    public bool FileExplorerContextMenusEnabled
    {
        get => _fileExplorerContextMenusEnabled;
        set => SetAndSave(ref _fileExplorerContextMenusEnabled, value);
    }
    #endregion FileExplorerContextMenusEnabled

    #region IncludePreRelease
    private bool _includePreRelease;
    public bool IncludePreRelease
    {
        get => _includePreRelease;
        set => SetAndSave(ref _includePreRelease, value);
    }
    #endregion IncludePreRelease

    #region CheckForUpdatesOnStartup
    private bool _checkForUpdatesOnStartup;
    public bool CheckForUpdatesOnStartup
    {
        get => _checkForUpdatesOnStartup;
        set => SetAndSave(ref _checkForUpdatesOnStartup, value);
    }
    #endregion CheckForUpdatesOnStartup

    #region StartingWhenCreateHashTask
    private bool _startingWhenCreateHashTask;
    public bool StartingWhenCreateHashTask
    {
        get => _startingWhenCreateHashTask;
        set => SetAndSave(ref _startingWhenCreateHashTask, value);
    }
    #endregion
}
