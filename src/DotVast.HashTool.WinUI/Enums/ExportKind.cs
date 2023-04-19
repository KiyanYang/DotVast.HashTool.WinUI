// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Enums;

[Flags]
public enum ExportKind
{
    None = 0,

    Json = 0x001_0000,
    Text = 0x002_0000,

    HashTask = 0x000_0001,
}
