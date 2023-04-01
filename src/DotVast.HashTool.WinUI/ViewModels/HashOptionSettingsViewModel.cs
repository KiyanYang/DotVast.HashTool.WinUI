using System.Collections.ObjectModel;
using System.Diagnostics;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HashOptionSettingsViewModel : ObservableRecipient, IViewModel, INavigationAware
{
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public HashOptionSettingsViewModel(IPreferencesSettingsService preferencesSettingsService)
    {
        _preferencesSettingsService = preferencesSettingsService;
    }

    public ObservableCollection<HashOption> HashOptions => _preferencesSettingsService.HashOptions;

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<HashOptionSettingsViewModel, HashOptionIsEnabledChangedMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > PropertyChangedMessage[HashOption.IsEnabled]");
            Debug.WriteLine($"Hash.Name: {m.HashOption.Hash.Name}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabled}");
            await _preferencesSettingsService.SaveHashOptionsAsync();
        });
    }

    #endregion Messenger

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        IsActive = true;
        HashOptions.CollectionChanged += HashOptions_CollectionChanged;
    }

    void INavigationAware.OnNavigatedFrom()
    {
        IsActive = false;
        HashOptions.CollectionChanged -= HashOptions_CollectionChanged;
    }

    #endregion INavigationAware

    private void HashOptions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (sender is ObservableCollection<HashOption>
            && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            _preferencesSettingsService.SaveHashOptionsAsync();
        }
    }
}
