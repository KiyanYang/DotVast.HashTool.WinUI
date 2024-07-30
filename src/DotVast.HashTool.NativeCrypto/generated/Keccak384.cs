// <auto-generated/>

#if Benchmark || sha3

using DotVast.Hashing;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

public sealed partial class Keccak384 : IHasher
{
    private sealed class Keccak384Handle : HasherHandle
    {
        protected override void Free() => Keccak384_free(handle);
    }

    private readonly Keccak384Handle _handle = Keccak384_new();

    public int HashLengthInBytes => 48;

    public void Reset() => Keccak384_reset(_handle);

    public void Append(ReadOnlySpan<byte> source) => Keccak384_update(_handle, source, source.Length);

    public byte[] Finalize()
    {
        var ret = new byte[48];
        Keccak384_finalize(_handle, ret, 48);
        return ret;
    }

    [LibraryImport("native_crypto", EntryPoint = "Keccak384_new")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial Keccak384Handle Keccak384_new();

    [LibraryImport("native_crypto", EntryPoint = "Keccak384_reset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void Keccak384_reset(Keccak384Handle hasherHandle);

    [LibraryImport("native_crypto", EntryPoint = "Keccak384_update")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void Keccak384_update(Keccak384Handle hasherHandle, ReadOnlySpan<byte> input, int size);

    [LibraryImport("native_crypto", EntryPoint = "Keccak384_finalize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void Keccak384_finalize(Keccak384Handle hasherHandle, Span<byte> output, int size);

    [LibraryImport("native_crypto", EntryPoint = "Keccak384_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void Keccak384_free(nint hasherPtr);
}

#endif
