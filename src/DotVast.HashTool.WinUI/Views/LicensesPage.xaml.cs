using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class LicensesPage : Page, IView
{
    public LicensesViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public LicensesPage()
    {
        ViewModel = App.GetService<LicensesViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, Localization.Licenses_Title);
    }
}
