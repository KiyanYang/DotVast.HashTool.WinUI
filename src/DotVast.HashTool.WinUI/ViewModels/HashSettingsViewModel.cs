using System.Collections.ObjectModel;
using System.Diagnostics;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HashSettingsViewModel : ObservableRecipient, IViewModel, INavigationAware
{
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public HashSettingsViewModel(IPreferencesSettingsService preferencesSettingsService)
    {
        _preferencesSettingsService = preferencesSettingsService;
    }

    public ObservableCollection<HashSetting> HashSettings => _preferencesSettingsService.HashSettings;

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<HashSettingsViewModel, HashSettingIsEnabledForAppChangedMessage>(this, (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > HashSettingIsEnabledForAppChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabledForApp}");
            _preferencesSettingsService.SaveHashSetting(m.HashSetting);
        });

        Messenger.Register<HashSettingsViewModel, HashSettingIsEnabledForContextMenuChangedMessage>(this, (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > HashSettingIsEnabledForContextMenuChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabledForContextMenu}");
            _preferencesSettingsService.SaveHashSetting(m.HashSetting, true);
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
