using System.Security.Cryptography;

using CryptoBase.Abstractions.Digests;

namespace DotVast.HashTool.WinUI.Models.Hashes;

internal sealed class CryptoBaseToHashAlgorithmAdapter : HashAlgorithm
{
    private readonly IHash _hash;

    internal CryptoBaseToHashAlgorithmAdapter(IHash hash)
    {
        _hash = hash;
        HashSizeValue = _hash.Length * 8;
    }

    protected sealed override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.Update(array.AsSpan().Slice(ibStart, cbSize));

    protected sealed override byte[] HashFinal()
    {
        var hashValue = new byte[_hash.Length];
        _hash.GetHash(hashValue.AsSpan());
        return hashValue;
    }

    public sealed override void Initialize() =>
        _hash.Reset();

    protected sealed override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _hash.Dispose();
        }
        base.Dispose(disposing);
    }
}
