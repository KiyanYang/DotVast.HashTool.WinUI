// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class HashService : IHashService
{
    sealed record HashMetadata(
        string Name,
        string[]? Aliases = null,
        HashFormat Format = HashFormat.Base16Upper,
        bool IsEnabledForApp = false,
        bool IsEnabledForContextMenu = false);

    static readonly Dictionary<HashKind, HashMetadata> s_hashes = new()
    {
        [HashKind.CRC_32_V42] = new("CRC-32", Aliases: new[] { "CRC-32/ADCCP", "CRC-32/V-42", "CRC-32/XZ", "PKZIP", "CRC-32/ISO-HDLC" }, IsEnabledForApp: true),
        [HashKind.CRC_64_ECMA182] = new("CRC-64", Aliases: new[] { "CRC-64/ECMA-182" }, IsEnabledForApp: true),
        [HashKind.MD4] = new("MD4"),
        [HashKind.MD5] = new("MD5", IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.SHA1] = new("SHA-1", Aliases: new[] { "SHA1" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.SHA2_224] = new("SHA-224", Aliases: new[] { "SHA224" }),
        [HashKind.SHA2_256] = new("SHA-256", Aliases: new[] { "SHA256" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.SHA2_384] = new("SHA-384", Aliases: new[] { "SHA384" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.SHA2_512] = new("SHA-512", Aliases: new[] { "SHA512" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.SHA3_224] = new("SHA3-224"),
        [HashKind.SHA3_256] = new("SHA3-256", IsEnabledForApp: true),
        [HashKind.SHA3_384] = new("SHA3-384", IsEnabledForApp: true),
        [HashKind.SHA3_512] = new("SHA3-512", IsEnabledForApp: true),
        [HashKind.SM3] = new("SM3", IsEnabledForApp: true),
        [HashKind.BLAKE2b_160] = new("BLAKE2b-160"),
        [HashKind.BLAKE2b_256] = new("BLAKE2b-256"),
        [HashKind.BLAKE2b_384] = new("BLAKE2b-384"),
        [HashKind.BLAKE2b_512] = new("BLAKE2b-512"),
        [HashKind.BLAKE2s_128] = new("BLAKE2s-128"),
        [HashKind.BLAKE2s_160] = new("BLAKE2s-160"),
        [HashKind.BLAKE2s_224] = new("BLAKE2s-224"),
        [HashKind.BLAKE2s_256] = new("BLAKE2s-256"),
        [HashKind.BLAKE2bp] = new("BLAKE2bp"),
        [HashKind.BLAKE2sp] = new("BLAKE2sp", IsEnabledForApp: true),
        [HashKind.BLAKE3] = new("BLAKE3", IsEnabledForApp: true),
        [HashKind.XxHash32] = new("XXH32", Aliases: new[] { "xxHash32" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.XxHash64] = new("XXH64", Aliases: new[] { "xxHash64" }, IsEnabledForApp: true, IsEnabledForContextMenu: true),
        [HashKind.XxHash3] = new("XXH3", Aliases: new[] { "xxHash3" }, IsEnabledForApp: true),
        [HashKind.XxHash128] = new("XXH128", Aliases: new[] { "xxHash128" }, IsEnabledForApp: true),
        [HashKind.Keccak_224] = new("Keccak-224", Aliases: new[] { "Keccak224" }),
        [HashKind.Keccak_256] = new("Keccak-256", Aliases: new[] { "Keccak256" }),
        [HashKind.Keccak_288] = new("Keccak-288", Aliases: new[] { "Keccak288" }),
        [HashKind.Keccak_384] = new("Keccak-384", Aliases: new[] { "Keccak384" }),
        [HashKind.Keccak_512] = new("Keccak-512", Aliases: new[] { "Keccak512" }),
        [HashKind.RIPEMD_128] = new("RIPEMD-128", Aliases: new[] { "RIPEMD128" }),
        [HashKind.RIPEMD_160] = new("RIPEMD-160", Aliases: new[] { "RIPEMD160" }, IsEnabledForApp: true),
        [HashKind.RIPEMD_256] = new("RIPEMD-256", Aliases: new[] { "RIPEMD256" }),
        [HashKind.RIPEMD_320] = new("RIPEMD-320", Aliases: new[] { "RIPEMD320" }),
        [HashKind.QuickXor] = new("QuickXor", Aliases: new[] { "QuickXorHash" }, Format: HashFormat.Base64),
        [HashKind.Ed2k] = new("eD2k"),
        [HashKind.HAS_160] = new("HAS-160", Aliases: new[] { "HAS160" }),
    };

    public string GetName(HashKind hashKind)
    {
        return s_hashes[hashKind].Name;
    }

    public IReadOnlyList<HashKind> HashKinds { get; } = Enum.GetValues<HashKind>();

    public HashKind? GetHashKind(string hashName)
    {
        if (Enum.TryParse<HashKind>(hashName, out var kind))
        {
            return kind;
        }

        foreach (var (hashKind, hashMetadata) in s_hashes)
        {
            if (hashMetadata.Name.Equals(hashName, StringComparison.OrdinalIgnoreCase)
             || (hashMetadata.Aliases?.Any(alias => alias.Equals(hashName, StringComparison.OrdinalIgnoreCase)) ?? false))
            {
                return hashKind;
            }
        }

        return null;
    }

    public IEnumerable<HashKind?> GetHashKinds(IEnumerable<string> hashNames)
    {
        return hashNames.Select(GetHashKind);
    }

    public IEnumerable<HashSetting> GetMergedHashSettings(IList<HashSetting> hashSettings)
    {
        return HashKinds.Select(kind => Merge(kind, hashSettings));

        HashSetting Merge(HashKind hashMetadataKind, IList<HashSetting> hashSettings)
        {
            var hashMetadata = s_hashes[hashMetadataKind];
            var retHashSetting = new HashSetting()
            {
                Kind = hashMetadataKind,
                Format = hashMetadata.Format,
                IsEnabledForApp = hashMetadata.IsEnabledForApp,
                IsEnabledForContextMenu = hashMetadata.IsEnabledForContextMenu,
            };

            var newHashSetting = hashSettings.FirstOrDefault(x => x.Kind == hashMetadataKind);
            if (newHashSetting is null)
            {
                return retHashSetting;
            }

            retHashSetting.Format = newHashSetting.Format;
            retHashSetting.IsChecked = newHashSetting.IsChecked;
            retHashSetting.IsEnabledForApp = newHashSetting.IsEnabledForApp;
            retHashSetting.IsEnabledForContextMenu = newHashSetting.IsEnabledForContextMenu;

            return retHashSetting;
        }
    }
}
