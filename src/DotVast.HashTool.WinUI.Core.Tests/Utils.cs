// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Core.Tests;

internal static class Utils
{
    public static void Fill<T>(Span<T> span, ReadOnlySpan<T> values)
    {
        values.CopyTo(span);

        int readLength;
        for (var i = values.Length; i < span.Length; i += readLength)
        {
            readLength = Math.Min(i, span.Length - i);
            span[..readLength].CopyTo(span[i..]);
        }
    }
}
