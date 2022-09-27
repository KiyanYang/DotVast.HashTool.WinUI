using CommunityToolkit.Mvvm.Messaging.Messages;

using DotVast.HashTool.WinUI.Contracts.Services;

namespace DotVast.HashTool.WinUI.Models.Messages;

internal sealed class ComputeHashStatueChangedMessage : ValueChangedMessage<ComputeHashStatus>
{
    public ComputeHashStatueChangedMessage(ComputeHashStatus value) : base(value)
    {
    }
}
