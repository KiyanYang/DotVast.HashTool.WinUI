using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HashOptionSettingsPage : Page, IView
{
    private const double HashOptionGridViewItemMinWidth = 200;

    public HashOptionSettingsViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public HashOptionSettingsPage()
    {
        ViewModel = App.GetService<HashOptionSettingsViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, Localization.HashOptionSettings_Title);
    }

    private void ItemsWrapGrid_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        if (e.NewSize.Width != e.PreviousSize.Width && sender is ItemsWrapGrid itemsWrapGrid)
        {
            var columns = Math.Floor(e.NewSize.Width / HashOptionGridViewItemMinWidth);
            itemsWrapGrid.ItemWidth = e.NewSize.Width / columns;
        }
    }
}
