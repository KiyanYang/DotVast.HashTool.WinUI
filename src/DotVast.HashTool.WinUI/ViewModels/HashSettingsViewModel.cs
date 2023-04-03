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
        Messenger.Register<HashSettingsViewModel, HashSettingIsEnabledForAppChangedMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > HashSettingIsEnabledForAppChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabledForApp}");
            await _preferencesSettingsService.SaveHashSettingsAsync();
        });

        Messenger.Register<HashSettingsViewModel, HashSettingIsEnabledForContextMenuChangedMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > HashSettingIsEnabledForContextMenuChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabledForContextMenu}");
            await _preferencesSettingsService.SaveHashSettingsAsync();
        });
    }

    #endregion Messenger

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        IsActive = true;
        HashSettings.CollectionChanged += HashOptions_CollectionChanged;
    }

    void INavigationAware.OnNavigatedFrom()
    {
        IsActive = false;
        HashSettings.CollectionChanged -= HashOptions_CollectionChanged;
    }

    #endregion INavigationAware

    private void HashOptions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (sender is ObservableCollection<HashSetting>
            && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            _preferencesSettingsService.SaveHashSettingsAsync();
        }
    }
}
