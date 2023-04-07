using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HashSettingsViewModel : ObservableRecipient, IViewModel, INavigationAware
{
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public HashSettingsViewModel(IPreferencesSettingsService preferencesSettingsService)
    {
        _preferencesSettingsService = preferencesSettingsService;
    }

    public IReadOnlyList<HashSetting> HashSettings => _preferencesSettingsService.HashSettings;

    #region Messenger

    protected override void OnActivated()
    {
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
