// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

using DotVast.HashTool.NativeCrypto;
using DotVast.HashTool.WinUI.Core.Tests.Hashes;

namespace DotVast.HashTool.WinUI.Core.Tests.NativeCrypto;

public class SM3Tests : HashTestDriver<SM3Tests>, IHashTest<SM3Tests>
{
    public static HashAlgorithm Create() => new SM3();

    [Fact]
    public static void SM3_HashSize()
    {
        var sm3 = Create();

        Assert.Equal(32, sm3.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { "616263", "66c7f0f462eeedd9d1f2d46bdc10e4e24167c4875cf2f7a2297da02b8f4ba8e0" };
        yield return new object[] { "61626364616263646162636461626364616263646162636461626364616263646162636461626364616263646162636461626364616263646162636461626364", "debe9ff92275b8a138604889c18e5a4d6fdb70e5387e5765293dcba39c0c5732" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[0] = Convert.FromHexString((string)item[0]);
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}
