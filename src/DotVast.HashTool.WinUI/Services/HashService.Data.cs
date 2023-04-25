// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Services;

partial class HashService
{
    private sealed class HashSettingCore
    {
        public required string Name { get; init; }
        public string[] Alias { get; init; } = Array.Empty<string>();
        public HashFormat Format { get; init; } = HashFormat.Base16Upper;
        public bool IsChecked { get; init; }
        public bool IsEnabledForApp { get; init; }
        public bool IsEnabledForContextMenu { get; init; }
    }

    private static readonly Dictionary<HashKind, HashSettingCore> _hashes = new()
    {
        [HashKind.CRC_32_V42] = new()
        {
            Name = "CRC-32",
            Alias = new[] { "CRC-32/ADCCP", "CRC-32/V-42", "CRC-32/XZ", "PKZIP", "CRC-32/ISO-HDLC" },
            IsEnabledForApp = true,
        },
        [HashKind.CRC_64_ECMA182] = new()
        {
            Name = "CRC-64",
            Alias = new[] { "CRC-64/ECMA-182" },
            IsEnabledForApp = true,
        },
        [HashKind.MD4] = new()
        {
            Name = "MD4",
        },
        [HashKind.MD5] = new()
        {
            Name = "MD5",
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true,
        },
        [HashKind.SHA1] = new()
        {
            Name = "SHA-1",
            Alias = new[] { "SHA1" },
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true,
        },
        [HashKind.SHA2_224] = new()
        {
            Name = "SHA-224",
            Alias = new[] { "SHA224" },
        },
        [HashKind.SHA2_256] = new()
        {
            Name = "SHA-256",
            Alias = new[] { "SHA256" },
            IsChecked = true,
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true,
        },
        [HashKind.SHA2_384] = new()
        {
            Name = "SHA-384",
            Alias = new[] { "SHA384" },
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true,
        },
        [HashKind.SHA2_512] = new()
        {
            Name = "SHA-512",
            Alias = new[] { "SHA512" },
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true,
        },
        [HashKind.SHA3_224] = new()
        {
            Name = "SHA3-224",
        },
        [HashKind.SHA3_256] = new()
        {
            Name = "SHA3-256",
            IsEnabledForApp = true,
        },
        [HashKind.SHA3_384] = new()
        {
            Name = "SHA3-384",
            IsEnabledForApp = true,
        },
        [HashKind.SHA3_512] = new()
        {
            Name = "SHA3-512",
            IsEnabledForApp = true,
        },
        [HashKind.SM3] = new()
        {
            Name = "SM3",
            IsEnabledForApp = true,
        },
        [HashKind.BLAKE2b_160] = new()
        {
            Name = "BLAKE2b-160",
        },
        [HashKind.BLAKE2b_256] = new()
        {
            Name = "BLAKE2b-256",
        },
        [HashKind.BLAKE2b_384] = new()
        {
            Name = "BLAKE2b-384",
        },
        [HashKind.BLAKE2b_512] = new()
        {
            Name = "BLAKE2b-512",
        },
        [HashKind.BLAKE2s_128] = new()
        {
            Name = "BLAKE2s-128",
        },
        [HashKind.BLAKE2s_160] = new()
        {
            Name = "BLAKE2s-160",
        },
        [HashKind.BLAKE2s_224] = new()
        {
            Name = "BLAKE2s-224",
        },
        [HashKind.BLAKE2s_256] = new()
        {
            Name = "BLAKE2s-256",
        },
        [HashKind.BLAKE2bp] = new()
        {
            Name = "BLAKE2bp",
        },
        [HashKind.BLAKE2sp] = new()
        {
            Name = "BLAKE2sp",
            IsEnabledForApp = true,
        },
        [HashKind.BLAKE3] = new()
        {
            Name = "BLAKE3",
            IsEnabledForApp = true,
        },
        [HashKind.RIPEMD_128] = new()
        {
            Name = "RIPEMD-128",
            Alias = new[] { "RIPEMD128" },
        },
        [HashKind.RIPEMD_160] = new()
        {
            Name = "RIPEMD-160",
            Alias = new[] { "RIPEMD160" },
            IsEnabledForApp = true,
        },
        [HashKind.RIPEMD_256] = new()
        {
            Name = "RIPEMD-256",
            Alias = new[] { "RIPEMD256" },
        },
        [HashKind.RIPEMD_320] = new()
        {
            Name = "RIPEMD-320",
            Alias = new[] { "RIPEMD320" },
        },
        [HashKind.Keccak_224] = new()
        {
            Name = "Keccak-224",
            Alias = new[] { "Keccak224" },
        },
        [HashKind.Keccak_256] = new()
        {
            Name = "Keccak-256",
            Alias = new[] { "Keccak256" },
        },
        [HashKind.Keccak_288] = new()
        {
            Name = "Keccak-288",
            Alias = new[] { "Keccak288" },
        },
        [HashKind.Keccak_384] = new()
        {
            Name = "Keccak-384",
            Alias = new[] { "Keccak384" },
        },
        [HashKind.Keccak_512] = new()
        {
            Name = "Keccak-512",
            Alias = new[] { "Keccak512" },
        },
        [HashKind.XxHash32] = new()
        {
            Name = "XXH32",
            Alias = new[] { "xxHash32" },
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true
        },
        [HashKind.XxHash64] = new()
        {
            Name = "XXH64",
            Alias = new[] { "xxHash64" },
            IsEnabledForApp = true,
            IsEnabledForContextMenu = true
        },
        [HashKind.XxHash3] = new()
        {
            Name = "XXH3",
            Alias = new[] { "xxHash3" },
            IsEnabledForApp = true,
        },
        [HashKind.XxHash128] = new()
        {
            Name = "XXH128",
            Alias = new[] { "xxHash128" },
            IsEnabledForApp = true,
        },
        [HashKind.QuickXor] = new()
        {
            Name = "QuickXor",
            Alias = new[] { "QuickXorHash" },
            Format = HashFormat.Base64,
        },
        [HashKind.Ed2k] = new()
        {
            Name = "eD2k",
        },
        [HashKind.HAS_160] = new()
        {
            Name = "HAS-160",
            Alias = new[] { "HAS160" },
        }
    };
}
