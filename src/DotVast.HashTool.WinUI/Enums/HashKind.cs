using System.Text.Json.Serialization;

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
