using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.ApplicationModel.DataTransfer;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HomePage : Page, IView
{
    public HomeViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }

    private void Page_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Link;
        e.DragUIOverride.Caption = Localization.Home_DargToSetPath;
    }

    private async void Page_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        if (!e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            return;
        }

        var items = await e.DataView.GetStorageItemsAsync();
        if (items.Count > 0)
        {
            ViewModel.SetHashTaskContenFromPaths(items.Select(i => i.Path));
        }
    }

    private void InputtingContentFull_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox box)
        {
            box.Width = InputtingContent.ActualWidth;
        }
    }

    #region x:Bind Function

    public static string? GetHashTaskModeFontIconGlyph(HashTaskMode mode)
    {
        return mode switch
        {
            HashTaskMode.Files => "\uE7C3",
            HashTaskMode.Folder => "\uE8B7",
            _ => null,
        };
    }

    public static string? GetPickerFontIconGlyph(HashTaskMode mode)
    {
        return mode switch
        {
            HashTaskMode.Files => "\uE8E5",
            HashTaskMode.Folder => "\uE8DA",
            _ => null,
        };
    }

    #endregion x:Bind Function
}
