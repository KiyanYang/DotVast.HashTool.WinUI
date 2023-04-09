using System.Security.Cryptography;

using CryptoBase.Abstractions.Digests;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class CryptoBaseAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this IHash hash) =>
        new CryptoBaseAdapter(hash);
}

sealed file class CryptoBaseAdapter : HashAlgorithm
{
    private readonly IHash _hash;

    internal CryptoBaseAdapter(IHash hash)
    {
        _hash = hash;
        HashSizeValue = _hash.Length * 8;
    }

    protected sealed override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.Update(array.AsSpan(ibStart, cbSize));

    protected sealed override byte[] HashFinal()
    {
        var hashValue = new byte[_hash.Length];
        _hash.GetHash(hashValue.AsSpan());
        return hashValue;
    }

    public sealed override void Initialize() =>
        _hash.Reset();

    #region Finalizer, IDisposable

    private bool _disposed = false;

    ~CryptoBaseAdapter() => Dispose(false);

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _hash.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion Finalizer, IDisposable
}
