using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult
{
    /// <summary>
    /// 哈希结果的计算类型.
    /// </summary>
    public HashResultType? Type
    {
        get; set;
    }

    /// <summary>
    /// 哈希结果的计算内容.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 哈希结果.
    /// </summary>
    public IList<HashResultItem>? Data
    {
        get; set;
    }
}

public sealed class HashResultItem
{
    private readonly Hash _hash;

    /// <summary>
    /// 哈希算法名称.
    /// </summary>
    public string Name => _hash.Name;

    /// <summary>
    /// 哈希结果.
    /// </summary>
    public string Hash
    {
        get; set;
    }

    public HashResultItem(Hash hash, string value)
    {
        _hash = hash;
        Hash = value;
    }
}
