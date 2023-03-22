using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class SettingsPage : Page, IView
{
    public SettingsViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        Resources.AddExpression(() => ViewModel.RestartAppCommand);
        Resources.AddExpression(() => Localization.Command_RestartApp);
        InitializeComponent();
    }
}
