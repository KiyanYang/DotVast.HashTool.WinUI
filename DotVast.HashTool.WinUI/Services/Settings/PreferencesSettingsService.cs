using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Models;

using static DotVast.HashTool.WinUI.Constants;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class PreferencesSettingsService : BaseObservableSettings, IPreferencesSettingsService
{
    public override async Task InitializeAsync()
    {
        await InitializeHashOptions();
        _includePreRelease = await LoadAsync(nameof(IncludePreRelease), DefaultPreferencesSettings.IncludePreRelease);
        _checkForUpdatesOnStartup = await LoadAsync(nameof(CheckForUpdatesOnStartup), DefaultPreferencesSettings.CheckForUpdatesOnStartup);
        _startingWhenCreateHashTask = await LoadAsync(nameof(StartingWhenCreateHashTask), DefaultPreferencesSettings.StartingWhenCreateHashTask);
    }

    public override async Task StartupAsync()
    {
        await Task.CompletedTask;
    }

    #region HashOptions
    public ObservableCollection<HashOption> HashOptions { get; } = new();

    private async Task InitializeHashOptions()
    {
        var allHashes = GenericEnum.GetFieldValues<Hash>();
        var allHashOptions = allHashes.Select(i => new HashOption(i));

        // 反序列化时, HashOption 的属性 Hash 可能为 null
        var hashOptionsSettings = await LoadAsync<List<HashOption>>(nameof(HashOptions), new());
        var hashOptions = hashOptionsSettings.Where(i => i.Hash != null)
            .UnionBy(allHashOptions, hashOption => hashOption.Hash);

        foreach (var hashOption in hashOptions)
        {
            HashOptions.Add(hashOption);
        }
    }

    public async Task SaveHashOptionsAsync()
    {
        await SaveAsync(HashOptions, nameof(HashOptions));
    }
    #endregion HashOptions

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
