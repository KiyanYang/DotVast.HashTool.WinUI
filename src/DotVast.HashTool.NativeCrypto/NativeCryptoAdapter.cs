// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

using DotVast.Hashing;

namespace DotVast.HashTool.NativeCrypto;

public sealed class NativeCryptoAdapter(IHasher hasher) : HashAlgorithm
{
    private readonly IHasher _hasher = hasher;

    public override void Initialize() => _hasher.Reset();
    protected override void HashCore(byte[] array, int ibStart, int cbSize) => _hasher.Append(array.AsSpan(ibStart, cbSize));
    protected override byte[] HashFinal() => _hasher.GetCurrentHash();
}

public static class NativeCryptoAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this IHasher hash) =>
        new NativeCryptoAdapter(hash);
}
