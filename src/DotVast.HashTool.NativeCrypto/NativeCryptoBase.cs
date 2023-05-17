// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace DotVast.HashTool.NativeCrypto;

public abstract class NativeCryptoBase : HashAlgorithm
{
    protected readonly nuint _hasher;

    public NativeCryptoBase()
    {
        _hasher = New();
    }

    protected abstract nuint New();
    protected abstract void Reset();
    protected abstract void Update(in byte input, int size);
    protected abstract void Finalize(ref byte output, int size);
    protected abstract void Free();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Update(ReadOnlySpan<byte> source)
    {
        if (source.IsEmpty)
        {
            return;
        }

        Update(MemoryMarshal.GetReference(source), source.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Finalize(Span<byte> output)
    {
        Debug.Assert(output.Length == HashSize);
        Finalize(ref MemoryMarshal.GetReference(output), HashSize);
    }

    public sealed override void Initialize()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Reset();
    }

    protected sealed override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Update(new ReadOnlySpan<byte>(array, ibStart, cbSize));
    }

    protected sealed override void HashCore(ReadOnlySpan<byte> source)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Update(source);
    }

    protected sealed override byte[] HashFinal()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var ret = GC.AllocateUninitializedArray<byte>(HashSize);
        Finalize(ret);
        return ret;
    }

    protected sealed override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (destination.Length < HashSize)
        {
            bytesWritten = 0;
            return false;
        }

        Finalize(destination[..HashSize]);
        bytesWritten = HashSize;
        return true;
    }

    #region IDisposable

    private bool _disposed;

    ~NativeCryptoBase() => Dispose(false);

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Free();

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion IDisposable
}

internal static partial class NativeMethods
{
    private const string DllName = @"native_crypto";
}
