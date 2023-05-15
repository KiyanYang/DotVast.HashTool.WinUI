// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Text;

using DotVast.HashTool.NativeCrypto;
using DotVast.HashTool.WinUI.Core.Tests.Hashes;

namespace DotVast.HashTool.WinUI.Core.Tests.NativeCrypto;

// https://homes.esat.kuleuven.be/~bosselae/ripemd160.html
static file class RIPEMDTestData
{
    public static readonly byte[] Msg1;
    public static readonly byte[] Msg2;
    public static readonly byte[] Msg3;
    public static readonly byte[] Msg4;
    public static readonly byte[] Msg5;
    public static readonly byte[] Msg6;
    public static readonly byte[] Msg7;
    public static readonly byte[] Msg8;
    public static readonly byte[] Msg9;

    static RIPEMDTestData()
    {
        Msg1 = Create("", 1);
        Msg2 = Create("a", 1);
        Msg3 = Create("abc", 1);
        Msg4 = Create("message digest", 1);
        Msg5 = Create("abcdefghijklmnopqrstuvwxyz", 1);
        Msg6 = Create("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", 1);
        Msg7 = Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 1);
        Msg8 = Create("1234567890", 8);
        Msg9 = Create("a", 1_000_000);

        static byte[] Create(string singleMsg, int count)
        {
            var source = new byte[singleMsg.Length * count];
            Utils.Fill<byte>(source, Encoding.ASCII.GetBytes(singleMsg));
            return source;
        }
    }
}

public class RIPEMD128Tests : HashTestDriver<RIPEMD128Tests>, IHashTest<RIPEMD128Tests>
{
    public static HashAlgorithm Create() => new RIPEMD128();

    [Fact]
    public static void RIPEMD128_HashSize()
    {
        var hasher = new RIPEMD128();

        Assert.Equal(16, hasher.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { RIPEMDTestData.Msg1, "cdf26213a150dc3ecb610f18f6b38b46" };
        yield return new object[] { RIPEMDTestData.Msg2, "86be7afa339d0fc7cfc785e72f578d33" };
        yield return new object[] { RIPEMDTestData.Msg3, "c14a12199c66e4ba84636b0f69144c77" };
        yield return new object[] { RIPEMDTestData.Msg4, "9e327b3d6e523062afc1132d7df9d1b8" };
        yield return new object[] { RIPEMDTestData.Msg5, "fd2aa607f71dc8f510714922b371834e" };
        yield return new object[] { RIPEMDTestData.Msg6, "a1aa0689d0fafa2ddc22e88b49133a06" };
        yield return new object[] { RIPEMDTestData.Msg7, "d1e959eb179c911faea4624c60c5c702" };
        yield return new object[] { RIPEMDTestData.Msg8, "3f45ef194732c2dbb2c4a2c769795fa3" };
        yield return new object[] { RIPEMDTestData.Msg9, "4a7f5723f954eba1216c9d8f6320431f" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}

public class RIPEMD160Tests : HashTestDriver<RIPEMD160Tests>, IHashTest<RIPEMD160Tests>
{
    public static HashAlgorithm Create() => new RIPEMD160();

    [Fact]
    public static void RIPEMD160_HashSize()
    {
        var hasher = new RIPEMD160();

        Assert.Equal(20, hasher.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { RIPEMDTestData.Msg1, "9c1185a5c5e9fc54612808977ee8f548b2258d31" };
        yield return new object[] { RIPEMDTestData.Msg2, "0bdc9d2d256b3ee9daae347be6f4dc835a467ffe" };
        yield return new object[] { RIPEMDTestData.Msg3, "8eb208f7e05d987a9b044a8e98c6b087f15a0bfc" };
        yield return new object[] { RIPEMDTestData.Msg4, "5d0689ef49d2fae572b881b123a85ffa21595f36" };
        yield return new object[] { RIPEMDTestData.Msg5, "f71c27109c692c1b56bbdceb5b9d2865b3708dbc" };
        yield return new object[] { RIPEMDTestData.Msg6, "12a053384a9c0c88e405a06c27dcf49ada62eb2b" };
        yield return new object[] { RIPEMDTestData.Msg7, "b0e20b6e3116640286ed3a87a5713079b21f5189" };
        yield return new object[] { RIPEMDTestData.Msg8, "9b752e45573d4b39f4dbd3323cab82bf63326bfb" };
        yield return new object[] { RIPEMDTestData.Msg9, "52783243c1697bdbe16d37f97f68f08325dc1528" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}

public class RIPEMD256Tests : HashTestDriver<RIPEMD256Tests>, IHashTest<RIPEMD256Tests>
{
    public static HashAlgorithm Create() => new RIPEMD256();

    [Fact]
    public static void RIPEMD256_HashSize()
    {
        var hasher = new RIPEMD256();

        Assert.Equal(32, hasher.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { RIPEMDTestData.Msg1, "02ba4c4e5f8ecd1877fc52d64d30e37a2d9774fb1e5d026380ae0168e3c5522d" };
        yield return new object[] { RIPEMDTestData.Msg2, "f9333e45d857f5d90a91bab70a1eba0cfb1be4b0783c9acfcd883a9134692925" };
        yield return new object[] { RIPEMDTestData.Msg3, "afbd6e228b9d8cbbcef5ca2d03e6dba10ac0bc7dcbe4680e1e42d2e975459b65" };
        yield return new object[] { RIPEMDTestData.Msg4, "87e971759a1ce47a514d5c914c392c9018c7c46bc14465554afcdf54a5070c0e" };
        yield return new object[] { RIPEMDTestData.Msg5, "649d3034751ea216776bf9a18acc81bc7896118a5197968782dd1fd97d8d5133" };
        yield return new object[] { RIPEMDTestData.Msg6, "3843045583aac6c8c8d9128573e7a9809afb2a0f34ccc36ea9e72f16f6368e3f" };
        yield return new object[] { RIPEMDTestData.Msg7, "5740a408ac16b720b84424ae931cbb1fe363d1d0bf4017f1a89f7ea6de77a0b8" };
        yield return new object[] { RIPEMDTestData.Msg8, "06fdcc7a409548aaf91368c06a6275b553e3f099bf0ea4edfd6778df89a890dd" };
        yield return new object[] { RIPEMDTestData.Msg9, "ac953744e10e31514c150d4d8d7b677342e33399788296e43ae4850ce4f97978" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}

public class RIPEMD320Tests : HashTestDriver<RIPEMD320Tests>, IHashTest<RIPEMD320Tests>
{
    public static HashAlgorithm Create() => new RIPEMD320();

    [Fact]
    public static void RIPEMD320_HashSize()
    {
        var hasher = new RIPEMD320();

        Assert.Equal(40, hasher.HashSize);
    }

    public static IEnumerable<object[]> TestDataCore()
    {
        yield return new object[] { RIPEMDTestData.Msg1, "22d65d5661536cdc75c1fdf5c6de7b41b9f27325ebc61e8557177d705a0ec880151c3a32a00899b8" };
        yield return new object[] { RIPEMDTestData.Msg2, "ce78850638f92658a5a585097579926dda667a5716562cfcf6fbe77f63542f99b04705d6970dff5d" };
        yield return new object[] { RIPEMDTestData.Msg3, "de4c01b3054f8930a79d09ae738e92301e5a17085beffdc1b8d116713e74f82fa942d64cdbc4682d" };
        yield return new object[] { RIPEMDTestData.Msg4, "3a8e28502ed45d422f68844f9dd316e7b98533fa3f2a91d29f84d425c88d6b4eff727df66a7c0197" };
        yield return new object[] { RIPEMDTestData.Msg5, "cabdb1810b92470a2093aa6bce05952c28348cf43ff60841975166bb40ed234004b8824463e6b009" };
        yield return new object[] { RIPEMDTestData.Msg6, "d034a7950cf722021ba4b84df769a5de2060e259df4c9bb4a4268c0e935bbc7470a969c9d072a1ac" };
        yield return new object[] { RIPEMDTestData.Msg7, "ed544940c86d67f250d232c30b7b3e5770e0c60c8cb9a4cafe3b11388af9920e1b99230b843c86a4" };
        yield return new object[] { RIPEMDTestData.Msg8, "557888af5f6d8ed62ab66945c6d2a0a47ecd5341e915eb8fea1d0524955f825dc717e4a008ab2d42" };
        yield return new object[] { RIPEMDTestData.Msg9, "bdee37f4371e20646b8b0d862dda16292ae36f40965e8c8509e63d1dbddecc503e2b63eb9245bb66" };
    }

    public static object[] ConvertDataItem(object[] item)
    {
        item[1] = Convert.FromHexString((string)item[1]);
        return item;
    }
}
