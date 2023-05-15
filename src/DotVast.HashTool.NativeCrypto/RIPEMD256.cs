// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class RIPEMD256 : NativeCryptoBase
{
    public RIPEMD256()
    {
        HashSizeValue = 32;
    }

    protected override nuint New() => NativeMethods.ripemd256_new();
    protected override void Reset() => NativeMethods.ripemd256_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.ripemd256_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.ripemd256_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.ripemd256_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint ripemd256_new();

    [LibraryImport(DllName)]
    internal static partial void ripemd256_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void ripemd256_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd256_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd256_free(nuint ptr);
}
