using System.Runtime.CompilerServices;

namespace DotVast.HashTool.WinUI.Core.Helpers;

public static class FileAttributesExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAnyFlag(this FileAttributes value, FileAttributes flags)
    {
        return (value & flags) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileAttributes AddFlag(this FileAttributes value, FileAttributes flags)
    {
        return value | flags;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileAttributes RemoveFlag(this FileAttributes value, FileAttributes flags)
    {
        return value & ~flags;
    }
}
