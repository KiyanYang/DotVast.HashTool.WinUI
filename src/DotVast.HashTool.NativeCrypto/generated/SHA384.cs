// <auto-generated/>

#if Benchmark || sha2

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class SHA384 : IHasher
{
    private sealed class SHA384Handle : HasherHandle
    {
        protected override void Free() => sha384_free(handle);
    }

    private readonly SHA384Handle _handle = sha384_new();

    public int HashLengthInBytes => 48;

    public void Reset() => sha384_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => sha384_update(_handle, source, source.Length);

    public byte[] GetCurrentHash()
    {
        var ret = new byte[48];
        sha384_finalize(_handle, ret, 48);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "sha384_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SHA384Handle sha384_new();

    [LibraryImport("native_crypto", EntryPoint = "sha384_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha384_reset(SHA384Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "sha384_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha384_update(SHA384Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "sha384_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha384_finalize(SHA384Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "sha384_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha384_free(nint hasherPtr);
}

#endif
