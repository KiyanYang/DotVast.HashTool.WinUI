namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashOption : ObservableRecipient
{
    private Hash _hash;

    public Hash Hash
    {
        get => _hash;
        set => SetProperty(ref _hash, value);
    }

    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isEnabled;

    public HashOption(Hash hash, bool isChecked = false, bool isEnabled = true)
    {
        _hash = hash;
        _isChecked = isChecked;
        _isEnabled = isEnabled;
    }
}
