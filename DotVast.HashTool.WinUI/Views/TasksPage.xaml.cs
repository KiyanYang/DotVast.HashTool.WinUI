using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class TasksPage : Page
{
    public TasksViewModel ViewModel { get; }

    public TasksPage()
    {
        ViewModel = App.GetService<TasksViewModel>();
        Resources.AddExpression(() => ViewModel.SaveCommand);
        Resources.AddExpression(() => ViewModel.ShowResultCommand);
        Resources.AddExpression(() => Localization.Command_Save_Tip);
        InitializeComponent();
    }
}
