// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class RIPEMD320 : NativeCryptoBase
{
    public RIPEMD320()
    {
        HashSizeValue = 40;
    }

    protected override nuint New() => NativeMethods.ripemd320_new();
    protected override void Reset() => NativeMethods.ripemd320_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.ripemd320_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.ripemd320_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.ripemd320_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint ripemd320_new();

    [LibraryImport(DllName)]
    internal static partial void ripemd320_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void ripemd320_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd320_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd320_free(nuint ptr);
}
