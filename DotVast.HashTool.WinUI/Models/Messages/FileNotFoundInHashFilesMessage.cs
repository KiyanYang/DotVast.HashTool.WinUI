using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DotVast.HashTool.WinUI.Models.Messages;

internal sealed class FileNotFoundInHashFilesMessage : ValueChangedMessage<string>
{
    public FileNotFoundInHashFilesMessage(string filePath) : base(filePath)
    {
    }
}
