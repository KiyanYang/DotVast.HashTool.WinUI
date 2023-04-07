using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HashSettingsPage : Page, IView
{
    private const double HashOptionGridViewItemMinWidth = 240;

    public HashSettingsViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public HashSettingsPage()
    {
        ViewModel = App.GetService<HashSettingsViewModel>();
        Resources.AddByExpression(() => ViewModel.HashFormats);
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, Localization.HashSettingsPage_Title);
    }

    private void GridView_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        if (e.NewSize.Width != e.PreviousSize.Width && sender is GridView { ItemsPanelRoot: ItemsWrapGrid itemsWrapGrid })
        {
            var columns = Math.Floor(e.NewSize.Width / HashOptionGridViewItemMinWidth);
            itemsWrapGrid.ItemWidth = e.NewSize.Width / columns;
        }
    }
}
