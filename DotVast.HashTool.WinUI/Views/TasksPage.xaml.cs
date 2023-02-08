using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
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

    #region  x:Bind Function

    public static string GetSecondaryInformationText(HashTask hashTask)
    {
        const string separator = "  |  ";

        var mode = hashTask.Mode.ToString();
        var dateTime = $"{separator}{hashTask.DateTime:HH:mm:ss}";
        var encoding = hashTask.Mode == HashTaskMode.Text
            ? $"{separator}{hashTask.Encoding!.WebName.ToUpper()}"
            : string.Empty;
        var hashNames = $"{separator}{string.Join(", ", hashTask.SelectedHashs.Select(i => i.Name))}";

        return $"{mode}{dateTime}{encoding}{hashNames}";
    }

    public static Visibility GetHashTaskStateVisibility(HashTaskState state)
    {
        return IsProgressRingActive(state) ? Visibility.Collapsed : Visibility.Visible;
    }

    public static bool IsProgressRingIndeterminate(HashTaskState state)
    {
        return state != HashTaskState.Working;
    }

    public static bool IsProgressRingActive(HashTaskState state)
    {
        return state == HashTaskState.Working;
    }

    #endregion  x:Bind Function
}
