// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Text;

using DotVast.HashTool.WinUI.Core.Hashes;

namespace DotVast.HashTool.WinUI.Core.Tests.Hashes;

public class MD4Tests : HashTestDriver<MD4Tests>, IHashTest<MD4Tests>
{
    public static HashAlgorithm Create() => new MD4();

    [Fact]
    public static void MD4_HashSize()
    {
        var md4 = Create();

        Assert.Equal(128, md4.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return ["", "31d6cfe0d16ae931b73c59d7e0c089c0"];
        yield return ["a", "bde52cb31de33e46245e05fbdbd6fb24"];
        yield return ["abc", "a448017aaf21d8525fc10ae87aa6729d"];
        yield return ["message digest", "d9130a8164549fe818874806e1c7014b"];
        yield return ["abcdefghijklmnopqrstuvwxyz", "d79e1c308aa5bbcdeea8ed63df412da9"];
        yield return ["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "043f8582f241db351ce627e153e7f0e4"];
        yield return ["12345678901234567890123456789012345678901234567890123456789012345678901234567890", "e33b4ddc9c38f2199c3e7b164fcc0536"];
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[0] = Encoding.UTF8.GetBytes((string)item[0]);
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}
