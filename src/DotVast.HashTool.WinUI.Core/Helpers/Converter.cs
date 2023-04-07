using System.Runtime.CompilerServices;

namespace DotVast.HashTool.WinUI.Core.Helpers;

public static class Converter
{
    private const string LowerHexChars = "0123456789abcdef";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToLowerHexString(byte[] bytes)
    {
        return ToLowerHexString(bytes.AsSpan());
    }

    public static string ToLowerHexString(ReadOnlySpan<byte> bytes)
    {
        Span<char> result = bytes.Length > 64
            ? new char[bytes.Length * 2].AsSpan()
            : stackalloc char[bytes.Length * 2];

        int index = 0;
        foreach (var b in bytes)
        {
            result[index++] = LowerHexChars[b >> 4];
            result[index++] = LowerHexChars[b & 0xF];
        }
        return new string(result);
    }
}
