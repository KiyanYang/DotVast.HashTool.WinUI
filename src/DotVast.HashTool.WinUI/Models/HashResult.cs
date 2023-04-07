using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult
{
    /// <summary>
    /// 文件路径.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 哈希结果.
    /// </summary>
    public IReadOnlyList<HashResultItem>? Data { get; set; }
}

public readonly record struct HashResultItem(HashKind Kind, string Value)
{
    /// <summary>
    /// 名称. 仅用作界面显示, 不要用于内部调用.
    /// </summary>
    [JsonIgnore]
    public string Name { get; } = Kind.ToName();
}
