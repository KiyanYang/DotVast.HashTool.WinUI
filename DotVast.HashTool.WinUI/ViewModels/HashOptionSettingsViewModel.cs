using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;

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
        // PropertyChangedMessage[HashOption.IsEnabled]
        Messenger.Register<HashOptionSettingsViewModel, PropertyChangedMessage<bool>>(this, async (r, m) =>
        {
            if (m.Sender is HashOption hashOption && m.PropertyName == nameof(HashOption.IsEnabled))
            {
                Debug.WriteLine($"[{DateTime.Now}] HashOptionSettingsViewModel.Messenger > PropertyChangedMessage[HashOption.IsEnabled]");
                Debug.WriteLine($"Hash.Name: {hashOption.Hash.Name}");
                Debug.WriteLine($"IsEnabled: {hashOption.IsEnabled}");
                await _preferencesSettingsService.SaveHashOptionsAsync();
            }
        });
    }

    #endregion Messenger

    #region INavigationAware

    public void OnNavigatedTo(object? parameter)
    {
        IsActive = true;
        HashOptions.CollectionChanged += HashOptions_CollectionChanged;
    }

    public void OnNavigatedFrom()
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
