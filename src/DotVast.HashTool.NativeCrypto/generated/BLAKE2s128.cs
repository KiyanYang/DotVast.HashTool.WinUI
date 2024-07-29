// <auto-generated/>

#if Benchmark || blake2

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class BLAKE2s128 : IHasher
{
    private sealed class BLAKE2s128Handle : HasherHandle
    {
        protected override void Free() => blake2s128_free(handle);
    }

    private readonly BLAKE2s128Handle _handle = blake2s128_new();

    public int HashLengthInBytes => 16;

    public void Reset() => blake2s128_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => blake2s128_update(_handle, source, source.Length);

    public byte[] GetCurrentHash()
    {
        var ret = new byte[16];
        blake2s128_finalize(_handle, ret, 16);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "blake2s128_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial BLAKE2s128Handle blake2s128_new();

    [LibraryImport("native_crypto", EntryPoint = "blake2s128_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2s128_reset(BLAKE2s128Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "blake2s128_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2s128_update(BLAKE2s128Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake2s128_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2s128_finalize(BLAKE2s128Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake2s128_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2s128_free(nint hasherPtr);
}

#endif
