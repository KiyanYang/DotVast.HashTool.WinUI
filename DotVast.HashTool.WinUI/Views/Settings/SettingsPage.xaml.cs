using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        Resources.AddExpression(() => ViewModel.RestartAppCommand);
        Resources.AddExpression(() => Localization.Command_RestartApp);
        InitializeComponent();
    }
}
