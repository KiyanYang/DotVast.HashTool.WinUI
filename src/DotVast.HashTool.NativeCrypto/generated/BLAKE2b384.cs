// <auto-generated/>

#if Benchmark || blake2

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class BLAKE2b384 : IHasher
{
    private sealed class BLAKE2b384Handle : HasherHandle
    {
        protected override void Free() => blake2b384_free(handle);
    }

    private readonly BLAKE2b384Handle _handle = blake2b384_new();

    public int HashLengthInBytes => 48;

    public void Reset() => blake2b384_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => blake2b384_update(_handle, source, source.Length);

    public byte[] GetCurrentHash()
    {
        var ret = new byte[48];
        blake2b384_finalize(_handle, ret, 48);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "blake2b384_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial BLAKE2b384Handle blake2b384_new();

    [LibraryImport("native_crypto", EntryPoint = "blake2b384_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2b384_reset(BLAKE2b384Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "blake2b384_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2b384_update(BLAKE2b384Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake2b384_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2b384_finalize(BLAKE2b384Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "blake2b384_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void blake2b384_free(nint hasherPtr);
}

#endif
