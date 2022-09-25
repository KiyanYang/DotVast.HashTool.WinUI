namespace DotVast.HashTool.WinUI.Models.Controls;

public sealed partial class ButtonModel : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string _content = string.Empty;
}
