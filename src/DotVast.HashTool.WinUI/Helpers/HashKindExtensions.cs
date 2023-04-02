using System.Security.Cryptography;

using CryptoBase.Digests.SM3;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers.Hashes;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Helpers;

internal static class HashKindExtensions
{
    public static HashAlgorithm ToHashAlgorithm(this HashKind hashKind)
    {
        return hashKind switch
        {
            HashKind.CRC_32 => new System.IO.Hashing.Crc32().ToHashAlgorithm(reverse: true),
            HashKind.CRC_64 => new System.IO.Hashing.Crc64().ToHashAlgorithm(),
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
            HashKind.SM3 => new SM3Digest().ToHashAlgorithm(),
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
