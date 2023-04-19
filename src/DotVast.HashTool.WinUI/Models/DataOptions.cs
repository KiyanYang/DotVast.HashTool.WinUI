// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class DataOptions
{
    public Dictionary<HashKind, HashData>? Hashes { get; set; }
}

public sealed class HashData
{
    public string Name { get; set; } = string.Empty;
    public string[] Alias { get; set; } = Array.Empty<string>();
    public HashFormat Format { get; set; } = HashFormat.Base16Upper;
    public bool IsChecked { get; set; }
    public bool IsEnabledForApp { get; set; }
    public bool IsEnabledForContextMenu { get; set; }
}
