using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Services.Hash;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult
{
    public HashResultType? Type
    {
        get; set;
    }

    public string Content { get; set; } = string.Empty;

    public IList<HashResultItem>? Data
    {
        get; set;
    }
}

public sealed class HashResultType : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static HashResultType File { get; } = new("HashResultType_File".GetLocalized());

    /// <summary>
    /// 文本.
    /// </summary>
    public static HashResultType Text { get; } = new("HashResultType_Text".GetLocalized());

    private HashResultType(string name) : base(name) { }
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
