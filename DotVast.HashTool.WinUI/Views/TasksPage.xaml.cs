using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class TasksPage : Page
{
    public TasksViewModel ViewModel
    {
        get;
    }

    public TasksPage()
    {
        ViewModel = App.GetService<TasksViewModel>();
        InitializeComponent();
    }

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
}
