// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Buffers;
using System.Security.Cryptography;

using DotVast.Hashing;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class HashAlgorithmAdapterExtensions
{
    public static IHasher ToIHasher(this HashAlgorithm hash) =>
        new HashAlgorithmAdapter(hash);
}

sealed file class HashAlgorithmAdapter : IHasher
{
    private readonly HashAlgorithm _algorithm;

    public HashAlgorithmAdapter(HashAlgorithm algorithm)
    {
        _algorithm = algorithm;
    }

    public int HashLengthInBytes => _algorithm.HashSize / 8;

    public void Reset() => _algorithm.Initialize();

    public void Append(ReadOnlySpan<byte> source)
    {
        var array = ArrayPool<byte>.Shared.Rent(source.Length);
        source.CopyTo(array);
        _algorithm.TransformBlock(array, 0, source.Length, null, 0);
        ArrayPool<byte>.Shared.Return(array);
    }

    public byte[] Finalize()
    {
        _algorithm.TransformFinalBlock([], 0, 0);
        return _algorithm.Hash!;
    }
}
