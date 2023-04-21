// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class HashService : IHashService
{
    public string GetName(HashKind hashKind)
    {
        return _hashes[hashKind].Name;
    }

    public IReadOnlyList<HashKind> HashKinds { get; } = Enum.GetValues<HashKind>();

    public HashKind? GetHash(string hashName)
    {
        if (Enum.TryParse<HashKind>(hashName, out var hash))
        {
            return hash;
        }

        foreach (var kvp in _hashes)
        {
            if (kvp.Value.Name.Equals(hashName, StringComparison.OrdinalIgnoreCase)
             || kvp.Value.Alias.Any(h => StringComparer.OrdinalIgnoreCase.Equals(hashName, h)))
            {
                return kvp.Key;
            }
        }

        return null;
    }

    public HashKind[] GetHashes(IEnumerable<string> hashNames)
    {
        return hashNames.Select(GetHash).OfType<HashKind>().ToArray();
    }

    public IEnumerable<HashSetting> GetMergedHashSettings(IList<HashSetting> hashSettings)
    {
        return HashKinds.Select(kind => Merge(kind, hashSettings));

        HashSetting Merge(HashKind defaultHashSettingKind, IList<HashSetting> hashSettings)
        {
            var defaultHashSetting = _hashes[defaultHashSettingKind];
            var retHashSetting = new HashSetting()
            {
                Kind = defaultHashSettingKind,
                Format = defaultHashSetting.Format,
                IsChecked = defaultHashSetting.IsChecked,
                IsEnabledForApp = defaultHashSetting.IsEnabledForApp,
                IsEnabledForContextMenu = defaultHashSetting.IsEnabledForContextMenu,
            };

            var newHashSetting = hashSettings.FirstOrDefault(x => x.Kind == defaultHashSettingKind);
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
