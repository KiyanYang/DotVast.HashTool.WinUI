using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class LicensesPage : Page
{
    public LicensesViewModel ViewModel
    {
        get;
    }

    public LicensesPage()
    {
        ViewModel = App.GetService<LicensesViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, "Licenses_Header".GetLocalized());
    }
}
