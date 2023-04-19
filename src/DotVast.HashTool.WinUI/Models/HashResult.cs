// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult
{
    public HashResult(string path, IReadOnlyList<HashResultItem> data)
    {
        Path = path;
        Data = data;
    }

    /// <summary>
    /// 文件路径.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// 哈希结果.
    /// </summary>
    public IReadOnlyList<HashResultItem> Data { get; }
}

public readonly record struct HashResultItem(HashKind Kind, string Value)
{
    private readonly HashSetting _hashSetting = Kind.GetHashSetting();

    /// <summary>
    /// 名称.
    /// </summary>
    [JsonIgnore]
    public string Name => _hashSetting.Name;
}
