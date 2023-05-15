// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed class BLAKE3 : NativeCryptoBase
{
    public BLAKE3()
    {
        HashSizeValue = 32;
    }

    protected override nuint New() => NativeMethods.blake3_new();
    protected override void Reset() => NativeMethods.blake3_reset(_hasher);
    protected override void Update(in byte input, nuint size) => NativeMethods.blake3_update(_hasher, input, size);
    protected override void Finalize(ref byte output, nuint size) => NativeMethods.blake3_finalize(_hasher, ref output, size);
    protected override void Free() => NativeMethods.blake3_free(_hasher);
}

partial class NativeMethods
{
    [LibraryImport(DllName)]
    internal static partial nuint blake3_new();

    [LibraryImport(DllName)]
    internal static partial void blake3_reset(nuint ptr);

    [LibraryImport(DllName)]
    internal static partial void blake3_update(nuint ptr, in byte input, nuint size);

    [LibraryImport(DllName)]
    internal static partial void blake3_finalize(nuint ptr, ref byte output, nuint size);

    [LibraryImport(DllName)]
    internal static partial void blake3_free(nuint ptr);
}
