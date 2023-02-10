using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class TasksPage : Page
{
    public TasksViewModel ViewModel { get; }

    public TasksPage()
    {
        ViewModel = App.GetService<TasksViewModel>();
        Resources.TryAdd(() => ViewModel.SaveCommand);
        Resources.TryAdd(() => ViewModel.ShowResultCommand);
        Resources.TryAdd(() => Localization.Command_Save_Tip);
        InitializeComponent();
    }
}
