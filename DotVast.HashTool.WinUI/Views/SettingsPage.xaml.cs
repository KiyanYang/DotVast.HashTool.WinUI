using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void RestartAppButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        ViewModel.RestartApp();
}
