// <auto-generated/>

#if Benchmark || blake3

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class BLAKE3 : IHasher
{
    private sealed class BLAKE3Handle : HasherHandle
    {
        protected override void Free() => blake3_free(handle);
    }

    private readonly BLAKE3Handle _handle = blake3_new();

    public int HashLengthInBytes => 32;

    public void Reset() => blake3_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => blake3_update(_handle, source, source.Length);

    public byte[] Finalize()
    {
        var ret = new byte[32];
        blake3_finalize(_handle, ret, 32);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "blake3_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial BLAKE3Handle blake3_new();

    [LibraryImport("native_crypto", EntryPoint = "blake3_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake3_reset(BLAKE3Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "blake3_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake3_update(BLAKE3Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake3_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake3_finalize(BLAKE3Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake3_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake3_free(nint hasherPtr);
}

#endif
