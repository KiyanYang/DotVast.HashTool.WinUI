using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Options;

namespace DotVast.HashTool.WinUI.Services;

internal class HashService : IHashService
{
    private readonly Dictionary<HashKind, HashData> _hashes;

    public HashService(IOptions<DataOptions> dataOptions)
    {
        ArgumentNullException.ThrowIfNull(dataOptions.Value.Hashes);
        _hashes = dataOptions.Value.Hashes;
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

    public HashData GetHashData(HashKind hash)
    {
        return _hashes[hash];
    }

    public IEnumerable<HashSetting> GetMergedHashSettings(IList<HashSetting> hashSettings)
    {
        return HashKinds.Select(kind => Merge(kind, hashSettings));

        HashSetting Merge(HashKind defaultHashSettingKind, IList<HashSetting> hashSettings)
        {
            var retHashSetting = new HashSetting()
            {
                Kind = defaultHashSettingKind,
                IsChecked = _hashes[defaultHashSettingKind].IsChecked,
                IsEnabledForApp = _hashes[defaultHashSettingKind].IsEnabledForApp,
                IsEnabledForContextMenu = _hashes[defaultHashSettingKind].IsEnabledForContextMenu,
            };

            var newHashSetting = hashSettings.FirstOrDefault(x => x.Kind == defaultHashSettingKind);
            if (newHashSetting is null)
            {
                return retHashSetting;
            }

            retHashSetting.IsChecked = newHashSetting.IsChecked;
            retHashSetting.IsEnabledForApp = newHashSetting.IsEnabledForApp;
            retHashSetting.IsEnabledForContextMenu = newHashSetting.IsEnabledForContextMenu;

            return retHashSetting;
        }
    }
}
