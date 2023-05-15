// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class RIPEMD160 : NativeCryptoBase
{
    public RIPEMD160()
    {
        HashSizeValue = 20;
    }

    protected override nuint New() => NativeMethods.ripemd160_new();
    protected override void Reset() => NativeMethods.ripemd160_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.ripemd160_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.ripemd160_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.ripemd160_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint ripemd160_new();

    [LibraryImport(DllName)]
    internal static partial void ripemd160_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void ripemd160_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd160_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void ripemd160_free(nuint ptr);
}
