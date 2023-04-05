using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashTaskMode
{
    File,
    Folder,
}

internal static class HashTaskModeExtensions
{
    public static string ToDisplay(this HashTaskMode hashTaskMode)
    {
        return hashTaskMode switch
        {
            HashTaskMode.File => LocalizationEnum.HashTaskMode_File,
            HashTaskMode.Folder => LocalizationEnum.HashTaskMode_Folder,
            _ => throw new ArgumentOutOfRangeException(nameof(hashTaskMode)),
        };
    }
}
