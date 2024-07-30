// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.IO.Hashing;

using DotVast.Hashing;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class NonCryptographicHashAlgorithmAdapterExtensions
{
    public static IHasher ToIHasher(this NonCryptographicHashAlgorithm hash, bool reverse = false) =>
        new NonCryptographicHashAlgorithmAdapter(hash, reverse);
}

sealed file class NonCryptographicHashAlgorithmAdapter : IHasher
{
    private readonly NonCryptographicHashAlgorithm _hash;
    private readonly bool _reverse;

    internal NonCryptographicHashAlgorithmAdapter(NonCryptographicHashAlgorithm hash, bool reverse)
    {
        _hash = hash;
        _reverse = reverse;
    }

    public int HashLengthInBytes => _hash.HashLengthInBytes;

    public void Reset() => _hash.Reset();

    public void Append(ReadOnlySpan<byte> source) => _hash.Append(source);

    public byte[] Finalize()
    {
        var hashValue = _hash.GetCurrentHash();
        if (_reverse)
        {
            Array.Reverse(hashValue);
        }
        return hashValue;
    }
}
