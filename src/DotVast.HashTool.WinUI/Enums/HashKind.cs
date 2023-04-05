using System.Security.Cryptography;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Helpers.Hashes;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Enums;

// Don't change the enum name unless absolutely necessary.
// If the name is changed, it will destroy the deserialization and cause the app to start failure.
// The name is the same as the key in the appsettings.json:DataOptions:Hashes.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashKind
{
    CRC_32_V42,
    CRC_64_ECMA182,
    MD4,
    MD5,
    SHA1,
    SHA2_224,
    SHA2_256,
    SHA2_384,
    SHA2_512,
    SHA3_224,
    SHA3_256,
    SHA3_384,
    SHA3_512,
    SM3,
    BLAKE2b_160,
    BLAKE2b_256,
    BLAKE2b_384,
    BLAKE2b_512,
    BLAKE2s_128,
    BLAKE2s_160,
    BLAKE2s_224,
    BLAKE2s_256,
    BLAKE2bp,
    BLAKE2sp,
    BLAKE3,
    RIPEMD_128,
    RIPEMD_160,
    RIPEMD_256,
    RIPEMD_320,
    Keccak_224,
    Keccak_256,
    Keccak_288,
    Keccak_384,
    Keccak_512,
    XxHash32,
    XxHash64,
    XxHash3,
    XxHash128,
    QuickXor,
    Ed2k,
    HAS_160,
}

internal static class HashKindExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this HashKind hashKind)
    {
        return hashKind switch
        {
            HashKind.CRC_32_V42 => new System.IO.Hashing.Crc32().ToHashAlgorithm(reverse: true),
            HashKind.CRC_64_ECMA182 => new System.IO.Hashing.Crc64().ToHashAlgorithm(),
            HashKind.MD4 => HashLib4CSharp.Base.HashFactory.Crypto.CreateMD4().ToHashAlgorithm(),
            HashKind.MD5 => MD5.Create(),
            HashKind.SHA1 => SHA1.Create(),
            HashKind.SHA2_224 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA2_224().ToHashAlgorithm(),
            HashKind.SHA2_256 => SHA256.Create(),
            HashKind.SHA2_384 => SHA384.Create(),
            HashKind.SHA2_512 => SHA512.Create(),
            HashKind.SHA3_224 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_224().ToHashAlgorithm(),
            HashKind.SHA3_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_256().ToHashAlgorithm(),
            HashKind.SHA3_384 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_384().ToHashAlgorithm(),
            HashKind.SHA3_512 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_512().ToHashAlgorithm(),
            HashKind.SM3 => new CryptoBase.Digests.SM3.SM3Digest().ToHashAlgorithm(),
            HashKind.BLAKE2b_160 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2B_160().ToHashAlgorithm(),
            HashKind.BLAKE2b_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2B_256().ToHashAlgorithm(),
            HashKind.BLAKE2b_384 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2B_384().ToHashAlgorithm(),
            HashKind.BLAKE2b_512 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2B_512().ToHashAlgorithm(),
            HashKind.BLAKE2s_128 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2S_128().ToHashAlgorithm(),
            HashKind.BLAKE2s_160 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2S_160().ToHashAlgorithm(),
            HashKind.BLAKE2s_224 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2S_224().ToHashAlgorithm(),
            HashKind.BLAKE2s_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2S_256().ToHashAlgorithm(),
            HashKind.BLAKE2bp => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2BP(64, Array.Empty<byte>()).ToHashAlgorithm(),
            HashKind.BLAKE2sp => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2SP(32, Array.Empty<byte>()).ToHashAlgorithm(),
            HashKind.BLAKE3 => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake3_256().ToHashAlgorithm(),
            HashKind.RIPEMD_128 => HashLib4CSharp.Base.HashFactory.Crypto.CreateRIPEMD128().ToHashAlgorithm(),
            HashKind.RIPEMD_160 => HashLib4CSharp.Base.HashFactory.Crypto.CreateRIPEMD160().ToHashAlgorithm(),
            HashKind.RIPEMD_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateRIPEMD256().ToHashAlgorithm(),
            HashKind.RIPEMD_320 => HashLib4CSharp.Base.HashFactory.Crypto.CreateRIPEMD320().ToHashAlgorithm(),
            HashKind.Keccak_224 => HashLib4CSharp.Base.HashFactory.Crypto.CreateKeccak_224().ToHashAlgorithm(),
            HashKind.Keccak_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateKeccak_256().ToHashAlgorithm(),
            HashKind.Keccak_288 => HashLib4CSharp.Base.HashFactory.Crypto.CreateKeccak_288().ToHashAlgorithm(),
            HashKind.Keccak_384 => HashLib4CSharp.Base.HashFactory.Crypto.CreateKeccak_384().ToHashAlgorithm(),
            HashKind.Keccak_512 => HashLib4CSharp.Base.HashFactory.Crypto.CreateKeccak_512().ToHashAlgorithm(),
            HashKind.XxHash32 => new System.IO.Hashing.XxHash32().ToHashAlgorithm(),
            HashKind.XxHash64 => new System.IO.Hashing.XxHash64().ToHashAlgorithm(),
            HashKind.XxHash3 => new System.IO.Hashing.XxHash3().ToHashAlgorithm(),
            HashKind.XxHash128 => new System.IO.Hashing.XxHash128().ToHashAlgorithm(),
            HashKind.QuickXor => new QuickXorHash(),
            HashKind.Ed2k => new Ed2k(),
            HashKind.HAS_160 => HashLib4CSharp.Base.HashFactory.Crypto.CreateHAS160().ToHashAlgorithm(),
            _ => throw new NotImplementedException(),
        };
    }

    public static HashData ToHashData(this HashKind hashKind)
    {
        return App.GetService<IHashService>().GetHashData(hashKind);
    }
}
