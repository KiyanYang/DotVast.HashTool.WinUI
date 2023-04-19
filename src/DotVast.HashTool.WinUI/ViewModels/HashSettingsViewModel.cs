// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HashSettingsViewModel : SimpleObservableRecipient, IViewModel, INavigationAware
{
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public HashSettingsViewModel(IPreferencesSettingsService preferencesSettingsService)
    {
        _preferencesSettingsService = preferencesSettingsService;
    }

    public IReadOnlyList<HashSetting> HashSettings => _preferencesSettingsService.HashSettings;

    public IReadOnlyList<HashFormat> HashFormats { get; } = Enum.GetValues<HashFormat>();

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.RegisterV<HashSettingsViewModel, HashSetting, HashFormat>(this, EMT.HashSetting_Format, static (r, o, _) =>
        {
            r._preferencesSettingsService.SaveHashSetting(o);
        });

        Messenger.RegisterV<HashSettingsViewModel, HashSetting, bool>(this, EMT.HashSetting_IsEnabledForApp, static (r, o, _) =>
        {
            r._preferencesSettingsService.SaveHashSetting(o);
        });

        Messenger.RegisterV<HashSettingsViewModel, HashSetting, bool>(this, EMT.HashSetting_IsEnabledForContextMenu, static (r, o, _) =>
        {
            r._preferencesSettingsService.SaveHashSetting(o, true);
        });
    }

    #endregion Messenger

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        IsActive = true;
    }

    void INavigationAware.OnNavigatedFrom()
    {
        IsActive = false;
    }

    #endregion INavigationAware
}
