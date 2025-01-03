// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Views;

using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ShellViewModel : ObservableObject, IViewModel
{
    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public INotificationService NotificationService { get; }

    [ObservableProperty]
    public partial bool IsBackEnabled { get; set; }

    [ObservableProperty]
    public partial object? Selected { get; set; }

    public ShellViewModel(
        INavigationService navigationService,
        INavigationViewService navigationViewService,
        INotificationService notificationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        NotificationService = notificationService;
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
