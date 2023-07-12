// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashOption
{
    public HashKind Kind { get; set; }

    public HashFormat Format { get; set; }

    public override string ToString() => $"({Kind}, {Format})";
}
