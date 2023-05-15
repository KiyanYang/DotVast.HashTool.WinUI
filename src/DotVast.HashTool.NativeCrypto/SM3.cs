// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class SM3 : NativeCryptoBase
{
    public SM3()
    {
        HashSizeValue = 32;
    }

    protected override nuint New() => NativeMethods.sm3_new();
    protected override void Reset() => NativeMethods.sm3_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.sm3_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.sm3_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.sm3_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint sm3_new();

    [LibraryImport(DllName)]
    internal static partial void sm3_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void sm3_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void sm3_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void sm3_free(nuint ptr);
}
