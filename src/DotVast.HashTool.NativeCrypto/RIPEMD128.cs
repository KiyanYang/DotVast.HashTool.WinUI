// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class RIPEMD128 : NativeCryptoBase
{
    public RIPEMD128()
    {
        HashSizeValue = 16;
    }

    protected override nuint New() => NativeMethods.ripemd128_new();
    protected override void Reset() => NativeMethods.ripemd128_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.ripemd128_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.ripemd128_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.ripemd128_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint ripemd128_new();

    [LibraryImport(DllName)]
    internal static partial void ripemd128_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void ripemd128_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd128_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd128_free(nuint ptr);
}
