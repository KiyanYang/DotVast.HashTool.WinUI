// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Enums;

/// <summary>
/// 哈希结果的表示格式.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashFormat
{
    Base16Upper,
    Base16Lower,
    Base64,
}

internal static class HashFormatExtensions
{
    // Don't translate this.
    public static string ToDisplay(HashFormat hashFormat)
    {
        return hashFormat switch
        {
            HashFormat.Base16Upper => "Hex Upper",
            HashFormat.Base16Lower => "Hex Lower",
            HashFormat.Base64 => "Base64",
            _ => throw new ArgumentOutOfRangeException(nameof(hashFormat)),
        };
    }
}
