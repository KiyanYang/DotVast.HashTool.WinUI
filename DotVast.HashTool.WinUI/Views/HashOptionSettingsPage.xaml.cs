using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HashOptionSettingsPage : Page
{
    public HashOptionSettingsViewModel ViewModel
    {
        get;
    }

    public HashOptionSettingsPage()
    {
        ViewModel = App.GetService<HashOptionSettingsViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, Localization.HashOptionSettingsPage_Header);
    }
}
