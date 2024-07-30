// <auto-generated/>

#if Benchmark || sha3

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class Keccak224 : IHasher
{
    private sealed class Keccak224Handle : HasherHandle
    {
        protected override void Free() => keccak224_free(handle);
    }

    private readonly Keccak224Handle _handle = keccak224_new();

    public int HashLengthInBytes => 28;

    public void Reset() => keccak224_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => keccak224_update(_handle, source, source.Length);

    public byte[] Finalize()
    {
        var ret = new byte[28];
        keccak224_finalize(_handle, ret, 28);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "keccak224_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial Keccak224Handle keccak224_new();

    [LibraryImport("native_crypto", EntryPoint = "keccak224_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void keccak224_reset(Keccak224Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "keccak224_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void keccak224_update(Keccak224Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "keccak224_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void keccak224_finalize(Keccak224Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "keccak224_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void keccak224_free(nint hasherPtr);
}

#endif
