using DotVast.HashTool.WinUI.Services.Hash;

namespace DotVast.HashTool.WinUI.Models;

public partial class HashOption : ObservableObject
{
    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    private Hash _hash;

    public HashOption(bool isChecked, Hash hash)
    {
        _isChecked = isChecked;
        _hash = hash;
    }
}
