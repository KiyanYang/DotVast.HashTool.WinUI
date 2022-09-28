namespace DotVast.HashTool.WinUI.Models.Controls;

public sealed partial class ProgressBarModel : ObservableObject
{
    [ObservableProperty]
    private double _min;

    [ObservableProperty]
    private double _max;

    [ObservableProperty]
    private double _val;

    [ObservableProperty]
    private bool _showPaused;
}
