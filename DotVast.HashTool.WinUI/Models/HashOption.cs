namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashOption : ObservableRecipient
{
    public Hash Hash { get; }

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isChecked;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isEnabled;

    public HashOption(Hash hash, bool isChecked = false, bool isEnabled = true)
    {
        Hash = hash;
        _isChecked = isChecked;
        _isEnabled = isEnabled;
    }
}
