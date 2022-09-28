using DotVast.HashTool.WinUI.Services.Hash;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashOption : ObservableObject
{
    [ObservableProperty]
    private Hash _hash;

    [ObservableProperty]
    private bool _isChecked;

    public HashOption(Hash hash, bool isChecked = false)
    {
        _hash = hash;
        _isChecked = isChecked;
    }
}
