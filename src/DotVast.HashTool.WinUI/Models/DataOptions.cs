using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public class DataOptions
{
    public Dictionary<HashKind, HashData>? Hashes { get; set; }
}

public class HashData
{
    public string Name { get; set; } = string.Empty;
    public string[] Alias { get; set; } = Array.Empty<string>();
    public HashFormat Format { get; set; } = HashFormat.Base16;
    public bool IsEnabledForApp { get; set; }
    public bool IsEnabledForContextMenu { get; set; }
}
