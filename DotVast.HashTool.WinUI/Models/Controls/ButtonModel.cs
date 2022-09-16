using DotVast.HashTool.WinUI.Helpers;

namespace DotVast.HashTool.WinUI.Models.Controls;

public partial class ButtonModel : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string _uid = string.Empty;

    [ObservableProperty]
    private string _content = string.Empty;

    partial void OnUidChanged(string value)
    {
        Content = value.GetLocalized();
    }
}
