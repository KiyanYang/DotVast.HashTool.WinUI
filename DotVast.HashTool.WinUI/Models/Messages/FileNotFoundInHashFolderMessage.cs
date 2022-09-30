using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DotVast.HashTool.WinUI.Models.Messages;

internal sealed class FileNotFoundInHashFolderMessage : ValueChangedMessage<string>
{
    public FileNotFoundInHashFolderMessage(string filePath) : base(filePath)
    {
    }
}
