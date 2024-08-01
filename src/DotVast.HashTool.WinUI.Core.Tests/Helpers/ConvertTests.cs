// Copyright (c) Kiyan Yang.
// Copyright (c) .NET Foundation and Contributors.
// Licensed under the MIT License.

using System.Text;

using Convert = DotVast.HashTool.WinUI.Core.Helpers.Convert;

namespace DotVast.HashTool.WinUI.Core.Tests.Helpers;

public class ConverterLowerHexStringTests
{
    [Fact]
    public static void KnownByteSequence()
    {
        byte[] inputBytes = [0x00, 0x01, 0x02, 0xFD, 0xFE, 0xFF];
        Assert.Equal("000102fdfeff", Convert.ToLowerHexString(inputBytes));
    }

    [Fact]
    public static void CompleteValueRange()
    {
        byte[] values = new byte[256];
        StringBuilder sb = new StringBuilder(256);
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (byte)i;
            sb.Append($"{i:x2}");
        }

        Assert.Equal(sb.ToString(), Convert.ToLowerHexString(values));
    }

    public static IEnumerable<object[]> ToHexStringTestData()
    {
        yield return [(byte[])[], ""];
        yield return [(byte[])[0x00], "00"];
        yield return [(byte[])[0x01], "01"];
        yield return [(byte[])[0xFF], "ff"];
        yield return [(byte[])[0x00, 0x00], "0000"];
        yield return [(byte[])[0xAB, 0xCD], "abcd"];
        yield return [(byte[])[0xFF, 0xFF], "ffff"];
        yield return [(byte[])[0x00, 0x00, 0x00], "000000"];
        yield return [(byte[])[0x01, 0x02, 0x03], "010203"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF], "ffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00], "00000000"];
        yield return [(byte[])[0xAB, 0xCD, 0xEF, 0x12], "abcdef12"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF], "ffffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00, 0x00], "0000000000"];
        yield return [(byte[])[0xAB, 0xCD, 0xEF, 0x12, 0x34], "abcdef1234"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF, 0xFF], "ffffffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00, 0x00, 0x00], "000000000000"];
        yield return [(byte[])[0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56], "abcdef123456"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF], "ffffffffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00], "00000000000000"];
        yield return [(byte[])[0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78], "abcdef12345678"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF], "ffffffffffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00], "0000000000000000"];
        yield return [(byte[])[0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90], "abcdef1234567890"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF], "ffffffffffffffff"];
        yield return [(byte[])[0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00], "000000000000000000"];
        yield return [(byte[])[0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09], "010203040506070809"];
        yield return [(byte[])[0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF], "ffffffffffffffffff"];
    }

    [Theory]
    [MemberData(nameof(ToHexStringTestData))]
    public static unsafe void ToLowerHexString(byte[] input, string expected)
    {
        string actual = Convert.ToLowerHexString(input);
        Assert.Equal(expected, actual);
    }
}
