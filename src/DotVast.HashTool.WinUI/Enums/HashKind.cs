// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

using DotVast.Hashing;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Helpers.Hashes;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Enums;

// Don't change the enum name unless absolutely necessary.
// If the name is changed, it will destroy the deserialization and cause the app to start failure.
[JsonConverter(typeof(JsonStringEnumConverter<HashKind>))]
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
    XxHash32,
    XxHash64,
    XxHash3,
    XxHash128,
    Keccak_224,
    Keccak_256,
    Keccak_384,
    Keccak_512,
    RIPEMD_128,
    RIPEMD_160,
    RIPEMD_256,
    RIPEMD_320,
    QuickXor,
    Ed2k,
    HAS_160,
}

internal static class HashKindExtensions
{
    private static IHashService? s_hashService;
    private static IHashService s_HashService =>
        s_hashService ??= App.GetService<IHashService>();

    private static IReadOnlyDictionary<HashKind, HashSetting>? s_hashSettings;
    private static IReadOnlyDictionary<HashKind, HashSetting> s_HashSettings =>
        s_hashSettings ??= App.GetService<IPreferencesSettingsService>().HashSettings.ToDictionary(hs => hs.Kind);

    public static IHasher ToIHasher(this HashKind hashKind)
    {
        return hashKind switch
        {
            HashKind.CRC_32_V42 => new System.IO.Hashing.Crc32().ToIHasher(reverse: true),
            HashKind.CRC_64_ECMA182 => new System.IO.Hashing.Crc64().ToIHasher(),
            HashKind.MD4 => HashLib4CSharp.Base.HashFactory.Crypto.CreateMD4().ToIHasher(),
            HashKind.MD5 => Hasher.CreateMD5(),
            HashKind.SHA1 => Hasher.CreateSHA1(),
            HashKind.SHA2_224 => Hasher.CreateSHA224(),
            HashKind.SHA2_256 => Hasher.CreateSHA256(),
            HashKind.SHA2_384 => Hasher.CreateSHA384(),
            HashKind.SHA2_512 => Hasher.CreateSHA512(),
            HashKind.SHA3_224 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_224().ToIHasher(),
            HashKind.SHA3_256 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_256().ToIHasher(),
            HashKind.SHA3_384 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_384().ToIHasher(),
            HashKind.SHA3_512 => HashLib4CSharp.Base.HashFactory.Crypto.CreateSHA3_512().ToIHasher(),
            HashKind.SM3 => Hasher.CreateSM3(),
            HashKind.BLAKE2b_160 => Hasher.CreateBLAKE2b160(),
            HashKind.BLAKE2b_256 => Hasher.CreateBLAKE2b256(),
            HashKind.BLAKE2b_384 => Hasher.CreateBLAKE2b384(),
            HashKind.BLAKE2b_512 => Hasher.CreateBLAKE2b512(),
            HashKind.BLAKE2s_128 => Hasher.CreateBLAKE2s128(),
            HashKind.BLAKE2s_160 => Hasher.CreateBLAKE2s160(),
            HashKind.BLAKE2s_224 => Hasher.CreateBLAKE2s224(),
            HashKind.BLAKE2s_256 => Hasher.CreateBLAKE2s256(),
            HashKind.BLAKE2bp => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2BP(64, []).ToIHasher(),
            HashKind.BLAKE2sp => HashLib4CSharp.Base.HashFactory.Crypto.CreateBlake2SP(32, []).ToIHasher(),
            HashKind.BLAKE3 => Hasher.CreateBLAKE3(),
            HashKind.XxHash32 => new System.IO.Hashing.XxHash32().ToIHasher(),
            HashKind.XxHash64 => new System.IO.Hashing.XxHash64().ToIHasher(),
            HashKind.XxHash3 => new System.IO.Hashing.XxHash3().ToIHasher(),
            HashKind.XxHash128 => new System.IO.Hashing.XxHash128().ToIHasher(),
            HashKind.Keccak_224 => Hasher.CreateKeccak224(),
            HashKind.Keccak_256 => Hasher.CreateKeccak256(),
            HashKind.Keccak_384 => Hasher.CreateKeccak384(),
            HashKind.Keccak_512 => Hasher.CreateKeccak512(),
            HashKind.RIPEMD_128 => Hasher.CreateRIPEMD128(),
            HashKind.RIPEMD_160 => Hasher.CreateRIPEMD160(),
            HashKind.RIPEMD_256 => Hasher.CreateRIPEMD256(),
            HashKind.RIPEMD_320 => Hasher.CreateRIPEMD320(),
            HashKind.QuickXor => Hasher.CreateQuickXor(),
            HashKind.Ed2k => new Ed2k(),
            HashKind.HAS_160 => HashLib4CSharp.Base.HashFactory.Crypto.CreateHAS160().ToIHasher(),
            _ => throw new ArgumentOutOfRangeException(nameof(hashKind)),
        };
    }

    public static string ToName(this HashKind hashKind)
    {
        return s_HashService.GetName(hashKind);
    }

    public static HashSetting GetHashSetting(this HashKind hashKind)
    {
        return s_HashSettings[hashKind];
    }
}
