// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashTaskMode
{
    Files,
    Folder,
}

internal static class HashTaskModeExtensions
{
    public static string ToDisplay(this HashTaskMode hashTaskMode)
    {
        return hashTaskMode switch
        {
            HashTaskMode.Files => LocalizationEnum.HashTaskMode_Files,
            HashTaskMode.Folder => LocalizationEnum.HashTaskMode_Folder,
            _ => throw new ArgumentOutOfRangeException(nameof(hashTaskMode)),
        };
    }
}
