namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTaskCheckable : ObservableRecipient
{
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isChecked;

    public HashTask HashTask { get; }

    public HashTaskCheckable(HashTask hashTask, bool isChecked)
    {
        HashTask = hashTask;
        _isChecked = isChecked;
    }
}
