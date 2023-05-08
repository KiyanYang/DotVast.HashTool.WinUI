// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

using HashLib4CSharp.Interfaces;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class HashLib4CSharpAdapterExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this IHash hash) =>
        new HashLib4CSharpAdapter(hash);
}

sealed file class HashLib4CSharpAdapter : HashAlgorithm
{
    private readonly IHash _hash;

    public HashLib4CSharpAdapter(IHash hash)
    {
        _hash = hash;
        HashSizeValue = _hash.HashSize * 8;
        Initialize();
    }

    public override void Initialize() =>
        _hash.Initialize();

    protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _hash.TransformByteSpan(new ReadOnlySpan<byte>(array, ibStart, cbSize));

    protected override void HashCore(ReadOnlySpan<byte> source) =>
        _hash.TransformByteSpan(source);

    protected override byte[] HashFinal() =>
        _hash.TransformFinal().GetBytes();
}
