using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Views;

using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ShellViewModel : ObservableObject
{
    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    [ObservableProperty]
    private bool _isBackEnabled;

    [ObservableProperty]
    private object? _selected;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
