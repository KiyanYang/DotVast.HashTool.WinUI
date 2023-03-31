using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

using CryptoBase.Digests.SM3;

using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Helpers.Hashes;
using DotVast.HashTool.WinUI.Helpers.JsonConverters;

using HashLib4CSharp.Checksum;

using static HashLib4CSharp.Base.HashFactory.Checksum.CRC;
using static HashLib4CSharp.Base.HashFactory.Crypto;

using Crypto = System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(HashJsonConverter))]
public sealed class Hash : GenericEnum<string>
{
    public string Name { get; }
    public HashFormat Format { get; }

    // CRC
    public static readonly Hash CRC32 = new("CRC32");
    public static readonly Hash CRC64 = new("CRC64");

    // MD
    public static readonly Hash MD4 = new("MD4");
    public static readonly Hash MD5 = new("MD5");

    // SHA1
    public static readonly Hash SHA1 = new("SHA1");

    // SHA2
    public static readonly Hash SHA224 = new("SHA-224");
    public static readonly Hash SHA256 = new("SHA-256");
    public static readonly Hash SHA384 = new("SHA-384");
    public static readonly Hash SHA512 = new("SHA-512");

    // SHA3
    public static readonly Hash SHA3_224 = new("SHA3-224");
    public static readonly Hash SHA3_256 = new("SHA3-256");
    public static readonly Hash SHA3_384 = new("SHA3-384");
    public static readonly Hash SHA3_512 = new("SHA3-512");

    // SM
    public static readonly Hash SM3 = new("SM3");

    // Blake2
    public static readonly Hash BLAKE2b_160 = new("BLAKE2b-160");
    public static readonly Hash BLAKE2b_256 = new("BLAKE2b-256");
    public static readonly Hash BLAKE2b_384 = new("BLAKE2b-384");
    public static readonly Hash BLAKE2b_512 = new("BLAKE2b-512");
    public static readonly Hash BLAKE2s_128 = new("BLAKE2s-128");
    public static readonly Hash BLAKE2s_160 = new("BLAKE2s-160");
    public static readonly Hash BLAKE2s_224 = new("BLAKE2s-224");
    public static readonly Hash BLAKE2s_256 = new("BLAKE2s-256");
    public static readonly Hash BLAKE2bp = new("BLAKE2bp");
    public static readonly Hash BLAKE2sp = new("BLAKE2sp");

    // Blake3
    public static readonly Hash BLAKE3 = new("BLAKE3");

    // RIPEMD
    public static readonly Hash RIPEMD_128 = new("RIPEMD-128");
    public static readonly Hash RIPEMD_160 = new("RIPEMD-160");
    public static readonly Hash RIPEMD_256 = new("RIPEMD-256");
    public static readonly Hash RIPEMD_320 = new("RIPEMD-320");

    // Keccak
    public static readonly Hash Keccak_224 = new("Keccak-224");
    public static readonly Hash Keccak_256 = new("Keccak-256");
    public static readonly Hash Keccak_288 = new("Keccak-288");
    public static readonly Hash Keccak_384 = new("Keccak-384");
    public static readonly Hash Keccak_512 = new("Keccak-512");

    // xxHash
    public static readonly Hash XxHash32 = new("xxHash32");
    public static readonly Hash XxHash64 = new("xxHash64");

    // Others
    public static readonly Hash QuickXor = new("QuickXor", HashFormat.Base64);
    public static readonly Hash Ed2k = new("eD2k");
    public static readonly Hash Has160 = new("HAS-160");

    public static Hash[] All => s_all ??= GetFieldValues<Hash>();
    private static Hash[]? s_all;

    private Hash(string name, HashFormat format = HashFormat.Base16) : base(name)
    {
        Name = name;
        Format = format;
    }

    public HashAlgorithm GetHashAlgorithm()
    {
        var hashAlgorithm = GetHashAlgorithm(this);
        Debug.Assert(hashAlgorithm is not null);
        return hashAlgorithm!;
    }

    public static HashAlgorithm? GetHashAlgorithm(Hash hash)
    {
        return hash switch
        {
            _ when hash == CRC32 => new System.IO.Hashing.Crc32().ToHashAlgorithm(reverse: true),
            _ when hash == CRC64 => new System.IO.Hashing.Crc64().ToHashAlgorithm(),
            _ when hash == MD4 => CreateMD4().ToHashAlgorithm(),
            _ when hash == MD5 => Crypto.MD5.Create(),
            _ when hash == SHA1 => Crypto.SHA1.Create(),
            _ when hash == SHA224 => CreateSHA2_224().ToHashAlgorithm(),
            _ when hash == SHA256 => Crypto.SHA256.Create(),
            _ when hash == SHA384 => Crypto.SHA384.Create(),
            _ when hash == SHA512 => Crypto.SHA512.Create(),
            _ when hash == SHA3_224 => CreateSHA3_224().ToHashAlgorithm(),
            _ when hash == SHA3_256 => CreateSHA3_256().ToHashAlgorithm(),
            _ when hash == SHA3_384 => CreateSHA3_384().ToHashAlgorithm(),
            _ when hash == SHA3_512 => CreateSHA3_512().ToHashAlgorithm(),
            _ when hash == SM3 => new SM3Digest().ToHashAlgorithm(),
            _ when hash == BLAKE2b_160 => CreateBlake2B_160().ToHashAlgorithm(),
            _ when hash == BLAKE2b_256 => CreateBlake2B_256().ToHashAlgorithm(),
            _ when hash == BLAKE2b_384 => CreateBlake2B_384().ToHashAlgorithm(),
            _ when hash == BLAKE2b_512 => CreateBlake2B_512().ToHashAlgorithm(),
            _ when hash == BLAKE2s_128 => CreateBlake2S_128().ToHashAlgorithm(),
            _ when hash == BLAKE2s_160 => CreateBlake2S_160().ToHashAlgorithm(),
            _ when hash == BLAKE2s_224 => CreateBlake2S_224().ToHashAlgorithm(),
            _ when hash == BLAKE2s_256 => CreateBlake2S_256().ToHashAlgorithm(),
            _ when hash == BLAKE2bp => CreateBlake2BP(64, Array.Empty<byte>()).ToHashAlgorithm(),
            _ when hash == BLAKE2sp => CreateBlake2SP(32, Array.Empty<byte>()).ToHashAlgorithm(),
            _ when hash == BLAKE3 => CreateBlake3_256().ToHashAlgorithm(),
            _ when hash == RIPEMD_128 => CreateRIPEMD128().ToHashAlgorithm(),
            _ when hash == RIPEMD_160 => CreateRIPEMD160().ToHashAlgorithm(),
            _ when hash == RIPEMD_256 => CreateRIPEMD256().ToHashAlgorithm(),
            _ when hash == RIPEMD_320 => CreateRIPEMD320().ToHashAlgorithm(),
            _ when hash == Keccak_224 => CreateKeccak_224().ToHashAlgorithm(),
            _ when hash == Keccak_256 => CreateKeccak_256().ToHashAlgorithm(),
            _ when hash == Keccak_288 => CreateKeccak_288().ToHashAlgorithm(),
            _ when hash == Keccak_384 => CreateKeccak_384().ToHashAlgorithm(),
            _ when hash == Keccak_512 => CreateKeccak_512().ToHashAlgorithm(),
            _ when hash == XxHash32 => new System.IO.Hashing.XxHash32().ToHashAlgorithm(),
            _ when hash == XxHash64 => new System.IO.Hashing.XxHash64().ToHashAlgorithm(),
            _ when hash == QuickXor => new QuickXorHash(),
            _ when hash == Ed2k => new Ed2k(),
            _ when hash == Has160 => CreateHAS160().ToHashAlgorithm(),
            _ => null,
        };
    }
}
