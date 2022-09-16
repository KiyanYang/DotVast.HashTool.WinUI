using DotVast.HashTool.WinUI.Services.Hash;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult
{
    public HashResultType Type
    {
        get; set;
    }

    public string Content { get; set; } = string.Empty;

    public IList<HashResultItem>? Data
    {
        get; set;
    }
}

public enum HashResultType
{
    /// <summary>
    /// 文件.
    /// </summary>
    File,

    /// <summary>
    /// 文本.
    /// </summary>
    Text,
}

public sealed class HashResultItem
{
    private readonly Hash _hash;

    public string Name => _hash.Name;

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
