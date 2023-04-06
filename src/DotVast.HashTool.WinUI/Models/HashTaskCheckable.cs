using CommunityToolkit.Mvvm.Messaging;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTaskCheckable : ObservableObject
{
    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.SendV<HashTaskCheckable, bool>(new(this, value), EMT.HashTaskCheckable_IsChecked);

    public HashTask HashTask { get; }

    public HashTaskCheckable(HashTask hashTask, bool isChecked)
    {
        HashTask = hashTask;
        _isChecked = isChecked;
    }
}
