using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTaskCheckable : ObservableObject
{
    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashTaskCheckableIsCheckedChangedMessage(this, value));

    public HashTask HashTask { get; }

    public HashTaskCheckable(HashTask hashTask, bool isChecked)
    {
        HashTask = hashTask;
        _isChecked = isChecked;
    }
}
