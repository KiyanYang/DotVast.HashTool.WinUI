using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DotVast.HashTool.WinUI.Models.Messages;

internal sealed class EditTaskMessage : ValueChangedMessage<HashTask>
{
    public EditTaskMessage(HashTask hashTask) : base(hashTask)
    {
    }
}
