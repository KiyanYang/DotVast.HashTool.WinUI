using System.Security.Cryptography;
using System.Text.Json.Serialization;

using CryptoBase.Digests.SM3;

using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Helpers.Hashes;

using HashLib4CSharp.Checksum;

using static HashLib4CSharp.Base.HashFactory.Checksum.CRC;
using static HashLib4CSharp.Base.HashFactory.Crypto;

using Crypto = System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Models;

[JsonConverter(typeof(JsonConverterFactoryForGenericEnumDerived))]
public sealed class Hash : GenericEnum<string>
{
    public string Name { get; }

    // CRC
    public static readonly Hash CRC32 = new("CRC32");

    // MD
    public static readonly Hash MD4 = new("MD4");
    public static readonly Hash MD5 = new("MD5");

    // SHA1
    public static readonly Hash SHA1 = new("SHA1");

    // SHA2
    public static readonly Hash SHA224 = new("SHA224");
    public static readonly Hash SHA256 = new("SHA256");
    public static readonly Hash SHA384 = new("SHA384");
    public static readonly Hash SHA512 = new("SHA512");

    // SHA3
    public static readonly Hash SHA3_224 = new("SHA3-224");
    public static readonly Hash SHA3_256 = new("SHA3-256");
    public static readonly Hash SHA3_384 = new("SHA3-384");
    public static readonly Hash SHA3_512 = new("SHA3-512");

    // SM
    public static readonly Hash SM3 = new("SM3");

    // Blake2B
    public static readonly Hash Blake2B_160 = new("Blake2B-160");
    public static readonly Hash Blake2B_256 = new("Blake2B-256");
    public static readonly Hash Blake2B_384 = new("Blake2B-384");
    public static readonly Hash Blake2B_512 = new("Blake2B-512");

    // Blake2S
    public static readonly Hash Blake2S_128 = new("Blake2S-128");
    public static readonly Hash Blake2S_160 = new("Blake2S-160");
    public static readonly Hash Blake2S_224 = new("Blake2S-224");
    public static readonly Hash Blake2S_256 = new("Blake2S-256");

    // Blake3
    public static readonly Hash Blake3 = new("Blake3");

    // RIPEMD
    public static readonly Hash RIPEMD128 = new("RIPEMD128");
    public static readonly Hash RIPEMD160 = new("RIPEMD160");
    public static readonly Hash RIPEMD256 = new("RIPEMD256");
    public static readonly Hash RIPEMD320 = new("RIPEMD320");

    // Keccak
    public static readonly Hash Keccak_224 = new("Keccak-224");
    public static readonly Hash Keccak_256 = new("Keccak-256");
    public static readonly Hash Keccak_288 = new("Keccak-288");
    public static readonly Hash Keccak_384 = new("Keccak-384");
    public static readonly Hash Keccak_512 = new("Keccak-512");

    // QuickXor
    public static readonly Hash QuickXor = new("QuickXor");

    public static Hash[] All => GetFieldValues<Hash>();

    private Hash(string name) : base(name)
    {
        Name = name;
    }

    public static HashAlgorithm? GetHashAlgorithm(Hash hash)
    {
        return hash switch
        {
            var h when h == Hash.CRC32 => CreateCRC(CRCModel.CRC32).ToHashAlgorithm(),
            var h when h == Hash.MD4 => CreateMD4().ToHashAlgorithm(),
            var h when h == Hash.MD5 => Crypto.MD5.Create(),
            var h when h == Hash.SHA1 => Crypto.SHA1.Create(),
            var h when h == Hash.SHA224 => CreateSHA2_224().ToHashAlgorithm(),
            var h when h == Hash.SHA256 => Crypto.SHA256.Create(),
            var h when h == Hash.SHA384 => Crypto.SHA384.Create(),
            var h when h == Hash.SHA512 => Crypto.SHA512.Create(),
            var h when h == Hash.SHA3_224 => CreateSHA3_224().ToHashAlgorithm(),
            var h when h == Hash.SHA3_256 => CreateSHA3_256().ToHashAlgorithm(),
            var h when h == Hash.SHA3_384 => CreateSHA3_384().ToHashAlgorithm(),
            var h when h == Hash.SHA3_512 => CreateSHA3_512().ToHashAlgorithm(),
            var h when h == Hash.SM3 => new SM3Digest().ToHashAlgorithm(),
            var h when h == Hash.Blake2B_160 => CreateBlake2B_160().ToHashAlgorithm(),
            var h when h == Hash.Blake2B_256 => CreateBlake2B_256().ToHashAlgorithm(),
            var h when h == Hash.Blake2B_384 => CreateBlake2B_384().ToHashAlgorithm(),
            var h when h == Hash.Blake2B_512 => CreateBlake2B_512().ToHashAlgorithm(),
            var h when h == Hash.Blake2S_128 => CreateBlake2S_128().ToHashAlgorithm(),
            var h when h == Hash.Blake2S_160 => CreateBlake2S_160().ToHashAlgorithm(),
            var h when h == Hash.Blake2S_224 => CreateBlake2S_224().ToHashAlgorithm(),
            var h when h == Hash.Blake2S_256 => CreateBlake2S_256().ToHashAlgorithm(),
            var h when h == Hash.Blake3 => CreateBlake3_256().ToHashAlgorithm(),
            var h when h == Hash.RIPEMD128 => CreateRIPEMD128().ToHashAlgorithm(),
            var h when h == Hash.RIPEMD160 => CreateRIPEMD160().ToHashAlgorithm(),
            var h when h == Hash.RIPEMD256 => CreateRIPEMD256().ToHashAlgorithm(),
            var h when h == Hash.RIPEMD320 => CreateRIPEMD320().ToHashAlgorithm(),
            var h when h == Hash.Keccak_224 => CreateKeccak_224().ToHashAlgorithm(),
            var h when h == Hash.Keccak_256 => CreateKeccak_256().ToHashAlgorithm(),
            var h when h == Hash.Keccak_288 => CreateKeccak_288().ToHashAlgorithm(),
            var h when h == Hash.Keccak_384 => CreateKeccak_384().ToHashAlgorithm(),
            var h when h == Hash.Keccak_512 => CreateKeccak_512().ToHashAlgorithm(),
            var h when h == Hash.QuickXor => new QuickXorHash(),
            _ => null,
        };
    }
}
