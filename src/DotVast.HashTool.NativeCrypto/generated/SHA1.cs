// <auto-generated/>

#if Benchmark || sha1

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class SHA1 : IHasher
{
    private sealed class SHA1Handle : HasherHandle
    {
        protected override void Free() => sha1_free(handle);
    }

    private readonly SHA1Handle _handle = sha1_new();

    public int HashLengthInBytes => 20;

    public void Reset() => sha1_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => sha1_update(_handle, source, source.Length);

    public byte[] Finalize()
    {
        var ret = new byte[20];
        sha1_finalize(_handle, ret, 20);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "sha1_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SHA1Handle sha1_new();

    [LibraryImport("native_crypto", EntryPoint = "sha1_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha1_reset(SHA1Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "sha1_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha1_update(SHA1Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "sha1_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha1_finalize(SHA1Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "sha1_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void sha1_free(nint hasherPtr);
}

#endif
