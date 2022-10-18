using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.ApplicationModel.DataTransfer;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        Resources.TryAdd(() => ViewModel.SaveHashOptionCommand);
        InitializeComponent();
    }

    private void Page_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Link;
        e.DragUIOverride.Caption = Localization.HomePage_DargToSetPath;
    }

    private async void Page_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
    {
        await ViewModel.SetHashTaskContenFromDrag(e.DataView);
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
            var m when m == HashTaskMode.File => "\xE7C3",
            var m when m == HashTaskMode.Folder => "\xE8B7",
            var m when m == HashTaskMode.Text => "\xE8C1",
            _ => null,
        };
    }

    public static string? GetPickerFontIconGlyph(HashTaskMode mode)
    {
        return mode switch
        {
            var m when m == HashTaskMode.File => "\xE8E5",
            var m when m == HashTaskMode.Folder => "\xE8DA",
            _ => null,
        };
    }

    #endregion x:Bind Function
}

public sealed class ComboBoxIconDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Normal { get; set; }
    public DataTemplate? DropDown { get; set; }

    protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
    {
        return container switch
        {
            ContentPresenter => Normal,
            _ => DropDown,
        };
    }
}
