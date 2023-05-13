// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Core.Tests.Hashes;

public abstract class HashTestDriver<T> where T : ITest<T>
{
    protected abstract HashAlgorithm Create();

    public static IEnumerable<object[]> HashTestData() => ITest<T>.TestData();

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void ComputeHash_Once(byte[] source, byte[] expected)
    {
        var hash = Create();

        var actual = hash.ComputeHash(source);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void ComputeHash_Reuse(byte[] source, byte[] expected)
    {
        var hash = Create();

        hash.ComputeHash(source);
        var actual = hash.ComputeHash(source);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void TransformBlock_Once(byte[] source, byte[] expected)
    {
        var hash = Create();

        hash.TransformBlock(source, 0, source.Length, null, 0);
        hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = hash.Hash;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void TransformBlock_Reuse(byte[] source, byte[] expected)
    {
        var hash = Create();

        hash.TransformBlock(source, 0, source.Length, null, 0);
        hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        hash.TransformBlock(source, 0, source.Length, null, 0);
        hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = hash.Hash;

        Assert.Equal(expected, actual);
    }
}
