// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.Hashing;

using HashLib4CSharp.Interfaces;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal static class HashLib4CSharpAdapterExtensions
{
    public static IHasher ToIHasher(this IHash hash) =>
        new HashLib4CSharpAdapter(hash);
}

sealed file class HashLib4CSharpAdapter : IHasher
{
    private readonly IHash _hash;

    public HashLib4CSharpAdapter(IHash hash)
    {
        _hash = hash;
        _hash.Initialize();
    }

    public int HashLengthInBytes => _hash.HashSize;

    public void Reset() => _hash.Initialize();

    public void Append(ReadOnlySpan<byte> source) => _hash.TransformByteSpan(source);

    public byte[] Finalize() => _hash.TransformFinal().GetBytes();
}
