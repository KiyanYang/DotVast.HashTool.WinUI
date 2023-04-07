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
