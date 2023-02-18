using System.IO.Hashing;
using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class NonCryptographicHashAlgorithmAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this NonCryptographicHashAlgorithm hash) =>
        new NonCryptographicHashAlgorithmAdapter(hash);
}

sealed file class NonCryptographicHashAlgorithmAdapter : HashAlgorithm
{
    private readonly NonCryptographicHashAlgorithm _hash;

    public NonCryptographicHashAlgorithmAdapter(NonCryptographicHashAlgorithm hash)
    {
        _hash = hash;
        HashSizeValue = hash.HashLengthInBytes * 8;
    }

    public override void Initialize() =>
        _hash.Reset();

    protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.Append(array.AsSpan(ibStart, cbSize));

    protected override byte[] HashFinal() =>
        _hash.GetCurrentHash();
}
