// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;

namespace DotVast.HashTool.WinUI.Core.Helpers;

public static class Converter
{
    private const string LowerHexChars = "0123456789abcdef";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToLowerHexString(byte[] bytes)
    {
        return string.Create(bytes.Length * 2, bytes, static (chars, args) =>
        {
            int index = 0;
            foreach (var b in args.AsSpan())
            {
                chars[index++] = LowerHexChars[b >> 4];
                chars[index++] = LowerHexChars[b & 0xF];
            }
        });
    }
}
