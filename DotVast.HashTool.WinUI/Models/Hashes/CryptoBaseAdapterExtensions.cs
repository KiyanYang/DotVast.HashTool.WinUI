using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Models.Hashes;

internal static class CryptoBaseAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this CryptoBase.Abstractions.Digests.IHash hash) =>
        new CryptoBaseToHashAlgorithmAdapter(hash);
}
