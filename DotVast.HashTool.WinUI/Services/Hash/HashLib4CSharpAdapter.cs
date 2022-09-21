using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Services.Hash;

internal static class HashLib4CSharpAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this HashLib4CSharp.Interfaces.IHash hash) =>
        HashLib4CSharp.Base.HashFactory.Adapter.CreateHashAlgorithmFromHash(hash);
}
