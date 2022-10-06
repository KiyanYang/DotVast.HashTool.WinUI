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

    protected override sealed void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.Update(array.AsSpan().Slice(ibStart, cbSize));

    protected override sealed byte[] HashFinal()
    {
        var hashValue = new byte[_hash.Length];
        _hash.GetHash(hashValue.AsSpan());
        return hashValue;
    }

    public override sealed void Initialize() =>
        _hash.Reset();

    protected override sealed void Dispose(bool disposing)
    {
        if (disposing)
        {
            _hash.Dispose();
        }
        base.Dispose(disposing);
    }
}
