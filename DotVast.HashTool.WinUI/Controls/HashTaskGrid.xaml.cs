using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Controls;

public sealed partial class HashTaskGrid : UserControl
{
    #region Dependency Properties

    public HashTask HashTask
    {
        get => (HashTask)GetValue(HashTaskProperty);
        set => SetValue(HashTaskProperty, value);
    }

    public static readonly DependencyProperty HashTaskProperty = DependencyProperty.Register(
       nameof(HashTask),
       typeof(HashTask),
       typeof(HashTaskGrid),
       new PropertyMetadata(null, OnHashTaskChanged));

    private static void OnHashTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not HashTaskGrid hashTaskGrid)
        {
            return;
        }

        hashTaskGrid.ViewModel.HashTask = e.NewValue as HashTask;
    }

    #endregion Dependency Properties

    public HashTaskGridViewModel ViewModel { get; set; }

    public HashTaskGrid()
    {
        ViewModel = App.GetService<HashTaskGridViewModel>();
        InitializeComponent();
    }

    #region x:Bind Function

    #region Icon

    private const string IconStart = "\uF5B0";
    private const string IconResume = "\uE768";
    private const string IconPause = "\uE769";
    private const string IconCancel = "\uE711";
    private const string IconEdit = "\uE70F";
    private const string IconDelete = "\uE74D";

    public static string GetStartBtnIcon() => IconStart;

    public static string GetResetBtnIcon(HashTaskState state)
    {
        return state == HashTaskState.Working ? IconPause : IconResume;
    }

    public static string GetCancelBtnIcon() => IconCancel;

    public static string GetEditBtnIcon() => IconEdit;

    public static string GetDeleteBtnIcon() => IconDelete;

    #endregion Icon

    #region Visibility

    public static Visibility GetHashTaskStateTextVisibility(HashTaskState state)
    {
        return GetProgressRingVisibility(state) switch
        {
            Visibility.Visible => Visibility.Collapsed,
            _ => Visibility.Visible,
        };
    }

    public static Visibility GetProgressRingVisibility(HashTaskState state)
    {
        return state == HashTaskState.Working || state == HashTaskState.Paused ? Visibility.Visible : Visibility.Collapsed;
    }

    public static bool IsProgressRingIndeterminate(HashTaskState state)
    {
        return state == HashTaskState.Paused;
    }

    #endregion Visibility

    public static string GetSecondaryInformationText(HashTask? hashTask)
    {
        if (hashTask is null)
        {
            return string.Empty;
        }

        const string separator = "  |  ";

        var mode = hashTask.Mode.ToString();
        var dateTime = $"{separator}{hashTask.DateTime:HH:mm:ss}";
        var encoding = hashTask.Mode == HashTaskMode.Text
            ? $"{separator}{hashTask.Encoding!.WebName.ToUpper()}"
            : string.Empty;
        var hashNames = $"{separator}{string.Join(", ", hashTask.SelectedHashs.Select(i => i.Name))}";

        return $"{mode}{dateTime}{encoding}{hashNames}";
    }

    public static string GetProgressText(double val, double max)
    {
        return $"{val / max * 100:F0}";
    }

    #endregion x:Bind Function
}
