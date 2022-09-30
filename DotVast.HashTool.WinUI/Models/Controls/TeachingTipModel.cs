namespace DotVast.HashTool.WinUI.Models.Controls;

public sealed partial class TeachingTipModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _subtitle = string.Empty;

    [ObservableProperty]
    private bool _isOpen;
}
