// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

using DotVast.HashTool.WinUI.Core.Hashes;

namespace DotVast.HashTool.WinUI.Core.Tests.Hashes;

public class QuickXorTests : HashTestDriver<QuickXorTests>, IHashTest<QuickXorTests>
{
    public static HashAlgorithm Create() => new QuickXor();

    [Fact]
    public static void QuickXor_HashSize()
    {
        var quickXor = Create();

        Assert.Equal(20, quickXor.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { "", "AAAAAAAAAAAAAAAAAAAAAAAAAAA=" };
        yield return new object[] { "517569636B586F72", "UahDGsawBiy8QQ4ACAAAAAAAAAA=" };
        yield return new object[] { "517569636B586F7248617368416C676F726974686D", "sKUxUsWt1vy6QQ5IHcMc0BAENpw=" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[0] = Convert.FromHexString((string)item[0]);
        item[1] = Convert.FromBase64String((string)item[1]);
        return item;
    }
}
