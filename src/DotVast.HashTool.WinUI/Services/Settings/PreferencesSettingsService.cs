using DotVast.HashTool.WinUI.Constants;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;

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
        _fileExplorerContextMenusEnabled = Load(nameof(FileExplorerContextMenusEnabled), DefaultPreferencesSettings.FileExplorerContextMenusEnabled);
        _includePreRelease = Load(nameof(IncludePreRelease), DefaultPreferencesSettings.IncludePreRelease);
        _checkForUpdatesOnStartup = Load(nameof(CheckForUpdatesOnStartup), DefaultPreferencesSettings.CheckForUpdatesOnStartup);
        _startingWhenCreateHashTask = Load(nameof(StartingWhenCreateHashTask), DefaultPreferencesSettings.StartingWhenCreateHashTask);
    }

    public override async Task StartupAsync()
    {
        await Task.CompletedTask;
    }

    #region HashSettings
    public IReadOnlyList<HashSetting> HashSettings { get; private set; } = Array.Empty<HashSetting>();

    private async Task InitializeHashSettings()
    {
        var hashSettingsFromLocalSettings = _hashService.HashKinds.Select(kind =>
                Load(SettingsContainerName.DataOptions_Hashes, kind.ToString(), default(HashSetting)))
            .OfType<HashSetting>()
            .ToArray();
        HashSettings = _hashService.GetMergedHashSettings(hashSettingsFromLocalSettings).ToArray();
        await Task.CompletedTask;
    }

    public void SaveHashSetting(HashSetting hashSetting, bool forContextMenu = false)
    {
        _localSettingsService.SaveSetting(SettingsContainerName.DataOptions_Hashes, hashSetting.Kind.ToString(), hashSetting);

        if (forContextMenu)
        {
            SaveHashNamesForContextMenu();
        }
    }

    private void SaveHashNamesForContextMenu()
    {
        var hashNamesForContexMenu = HashSettings.Where(h => h.IsEnabledForContextMenu).Select(h => h.Name);
        _localSettingsService.SaveSetting(SettingsContainerName.ContextMenu, "HashNames", hashNamesForContexMenu);
    }
    #endregion HashSettings

    #region FileExplorerContextMenusEnabled
    private bool _fileExplorerContextMenusEnabled;
    public bool FileExplorerContextMenusEnabled
    {
        get => _fileExplorerContextMenusEnabled;
        set => SetPropertyAndSave(SettingsContainerName.ContextMenu, "IsEnabled", value, ref _fileExplorerContextMenusEnabled);
    }
    #endregion FileExplorerContextMenusEnabled

    #region IncludePreRelease
    private bool _includePreRelease;
    public bool IncludePreRelease
    {
        get => _includePreRelease;
        set => SetPropertyAndSave(value, ref _includePreRelease);
    }
    #endregion IncludePreRelease

    #region CheckForUpdatesOnStartup
    private bool _checkForUpdatesOnStartup;
    public bool CheckForUpdatesOnStartup
    {
        get => _checkForUpdatesOnStartup;
        set => SetPropertyAndSave(value, ref _checkForUpdatesOnStartup);
    }
    #endregion CheckForUpdatesOnStartup

    #region StartingWhenCreateHashTask
    private bool _startingWhenCreateHashTask;
    public bool StartingWhenCreateHashTask
    {
        get => _startingWhenCreateHashTask;
        set => SetPropertyAndSave(value, ref _startingWhenCreateHashTask);
    }
    #endregion
}
