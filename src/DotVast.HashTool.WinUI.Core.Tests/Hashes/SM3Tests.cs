// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Core.Hashes;

namespace DotVast.HashTool.WinUI.Core.Tests.Hashes;

public class SM3Tests
{
    [Fact]
    public static void SM3_HashSize()
    {
        var sm3 = new SM3();

        Assert.Equal(32, sm3.HashSize);
    }

    public static IEnumerable<object[]> SM3HashTestData()
    {
        yield return new object[] { "616263", "66c7f0f462eeedd9d1f2d46bdc10e4e24167c4875cf2f7a2297da02b8f4ba8e0" };
        yield return new object[] { "61626364616263646162636461626364616263646162636461626364616263646162636461626364616263646162636461626364616263646162636461626364", "debe9ff92275b8a138604889c18e5a4d6fdb70e5387e5765293dcba39c0c5732" };
    }

    [Theory]
    [MemberData(nameof(SM3HashTestData))]
    public static void SM3_ComputeHash_Once(string input, string expected)
    {
        var inputBytes = Convert.FromHexString(input);
        var expectedBytes = Convert.FromHexString(expected);
        var sm3 = new SM3();

        var actual = sm3.ComputeHash(inputBytes);
        Assert.Equal(expectedBytes, actual);
    }

    [Theory]
    [MemberData(nameof(SM3HashTestData))]
    public static void SM3_ComputeHash_Reuse(string input, string expected)
    {
        var inputBytes = Convert.FromHexString(input);
        var expectedBytes = Convert.FromHexString(expected);
        var sm3 = new SM3();

        var actual = sm3.ComputeHash(inputBytes);
        Assert.Equal(expectedBytes, actual);

        actual = sm3.ComputeHash(inputBytes);
        Assert.Equal(expectedBytes, actual);
    }

    [Theory]
    [MemberData(nameof(SM3HashTestData))]
    public static void SM3_TransformBlock_Once(string input, string expected)
    {
        var inputBytes = Convert.FromHexString(input);
        var expectedBytes = Convert.FromHexString(expected);
        var sm3 = new SM3();

        sm3.TransformBlock(inputBytes, 0, inputBytes.Length, null, 0);
        sm3.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = sm3.Hash;

        Assert.Equal(expectedBytes, actual);
    }

    [Theory]
    [MemberData(nameof(SM3HashTestData))]
    public static void SM3_TransformBlock_Reuse(string input, string expected)
    {
        var inputBytes = Convert.FromHexString(input);
        var expectedBytes = Convert.FromHexString(expected);
        var sm3 = new SM3();

        sm3.TransformBlock(inputBytes, 0, inputBytes.Length, null, 0);
        sm3.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        sm3.TransformBlock(inputBytes, 0, inputBytes.Length, null, 0);
        sm3.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        var actual = sm3.Hash;

        Assert.Equal(expectedBytes, actual);
    }
}
