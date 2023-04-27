// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class HashLib4CSharpAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this HashLib4CSharp.Interfaces.IHash hash) =>
        HashLib4CSharp.Base.HashFactory.Adapter.CreateHashAlgorithmFromHash(hash);
}
