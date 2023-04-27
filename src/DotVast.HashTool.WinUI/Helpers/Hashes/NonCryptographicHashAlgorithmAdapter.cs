// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.IO.Hashing;
using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class NonCryptographicHashAlgorithmAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this NonCryptographicHashAlgorithm hash, bool reverse = false) =>
        new NonCryptographicHashAlgorithmAdapter(hash, reverse);
}

sealed file class NonCryptographicHashAlgorithmAdapter : HashAlgorithm
{
    private readonly NonCryptographicHashAlgorithm _hash;
    private readonly bool _reverse;

    internal NonCryptographicHashAlgorithmAdapter(NonCryptographicHashAlgorithm hash, bool reverse)
    {
        _hash = hash;
        _reverse = reverse;
        HashSizeValue = hash.HashLengthInBytes * 8;
    }

    public override void Initialize() =>
        _hash.Reset();

    protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.Append(new ReadOnlySpan<byte>(array, ibStart, cbSize));

    protected override void HashCore(ReadOnlySpan<byte> source) =>
        _hash.Append(source);

    protected override byte[] HashFinal()
    {
        var hashValue = _hash.GetCurrentHash();
        if (_reverse)
        {
            Array.Reverse(hashValue);
        }
        return hashValue;
    }
}
