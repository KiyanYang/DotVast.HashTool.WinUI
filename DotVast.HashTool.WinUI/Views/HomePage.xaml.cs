using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

using Windows.ApplicationModel.DataTransfer;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        Resources.TryAddFromVM(() => ViewModel.SaveHashOptionCommand);
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
}
