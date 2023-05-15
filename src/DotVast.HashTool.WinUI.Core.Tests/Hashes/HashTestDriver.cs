// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Core.Tests.Hashes;

public abstract class HashTestDriver<T> where T : IHashTest<T>
{
    public static IEnumerable<object[]> HashTestData() => ITest<T>.TestData();

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void ComputeHash_Once(byte[] source, byte[] expected)
    {
        using var hasher = T.Create();

        var actual = hasher.ComputeHash(source);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void ComputeHash_Reuse(byte[] source, byte[] expected)
    {
        using var hasher = T.Create();

        hasher.ComputeHash(source);
        var actual = hasher.ComputeHash(source);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void TransformBlock_Once(byte[] source, byte[] expected)
    {
        using var hasher = T.Create();

        hasher.TransformBlock(source, 0, source.Length, null, 0);
        hasher.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = hasher.Hash;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HashTestData))]
    public void TransformBlock_Reuse(byte[] source, byte[] expected)
    {
        using var hasher = T.Create();

        hasher.TransformBlock(source, 0, source.Length, null, 0);
        hasher.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        hasher.TransformBlock(source, 0, source.Length, null, 0);
        hasher.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = hasher.Hash;

        Assert.Equal(expected, actual);
    }
}
