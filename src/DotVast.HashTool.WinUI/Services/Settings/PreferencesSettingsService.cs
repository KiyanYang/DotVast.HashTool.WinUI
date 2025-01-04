// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Constants;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class PreferencesSettingsService(IHashService hashService) : BaseObservableSettings, IPreferencesSettingsService
{
    private readonly IHashService _hashService = hashService;

    public override Task InitializeAsync()
    {
        InitializeHashSettings();
        InitializeFileExplorerContextMenusEnabled();
        _fileAttributesToSkip = Load(nameof(FileAttributesToSkip), DefaultPreferencesSettings.FileAttributesToSkipWhenFolderMode);
        _includePreRelease = Load(nameof(IncludePreRelease), DefaultPreferencesSettings.IncludePreRelease);
        _checkForUpdatesOnStartup = Load(nameof(CheckForUpdatesOnStartup), DefaultPreferencesSettings.CheckForUpdatesOnStartup);
        _startingWhenCreateHashTask = Load(nameof(StartingWhenCreateHashTask), DefaultPreferencesSettings.StartingWhenCreateHashTask);

        return Task.CompletedTask;
    }

    public override Task StartupAsync()
    {
        RegisterMessages();
        return Task.CompletedTask;
    }

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.RegisterV<IPreferencesSettingsService, HashSetting, HashFormat>(this, EMT.HashSetting_Format, static (r, o, _) =>
        {
            r.SaveHashSetting(o);
        });
        WeakReferenceMessenger.Default.RegisterV<IPreferencesSettingsService, HashSetting, bool>(this, EMT.HashSetting_IsChecked, static (r, o, _) =>
        {
            r.SaveHashSetting(o);
        });
        WeakReferenceMessenger.Default.RegisterV<IPreferencesSettingsService, HashSetting, bool>(this, EMT.HashSetting_IsEnabledForApp, static (r, o, _) =>
        {
            r.SaveHashSetting(o);
        });

        WeakReferenceMessenger.Default.RegisterV<IPreferencesSettingsService, HashSetting, bool>(this, EMT.HashSetting_IsEnabledForContextMenu, static (r, o, _) =>
        {
            r.SaveHashSetting(o, true);
        });
    }

    #region HashSettings
    public IReadOnlyList<HashSetting> HashSettings { get; private set; } = [];

    private void InitializeHashSettings()
    {
        var hashSettingsFromLocalSettings = _hashService.HashKinds.Select(kind =>
                Load(SettingsContainerName.DataOptions_Hashes, kind.ToString(), default(HashSetting)))
            .OfType<HashSetting>()
            .ToArray();
        HashSettings = _hashService.GetMergedHashSettings(hashSettingsFromLocalSettings).ToArray();
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

    #region FileAttributesToSkip
    private FileAttributes _fileAttributesToSkip;
    public FileAttributes FileAttributesToSkip
    {
        get => _fileAttributesToSkip;
        set => SetPropertyAndSave(value, ref _fileAttributesToSkip);
    }
    #endregion FileAttributesToSkip

    #region FileExplorerContextMenusEnabled
    private void InitializeFileExplorerContextMenusEnabled()
    {
        _fileExplorerContextMenusEnabled = Load(SettingsContainerName.ContextMenu, "IsEnabled", DefaultPreferencesSettings.FileExplorerContextMenusEnabled);
    }
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
