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

    public HashKind[] AllHashes { get; } = Enum.GetValues<HashKind>();

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

    public IEnumerable<HashSetting> FillHashSettings(IEnumerable<HashSetting>? hashSettings)
    {
        if (hashSettings is null || !hashSettings.Any())
        {
            return AllHashes.Select(CreateHashSetting);
        }

        return FillNotEmptyHashSettings(hashSettings);
    }

    private IEnumerable<HashSetting> FillNotEmptyHashSettings(IEnumerable<HashSetting> hashSettings)
    {
        var hashSet = new HashSet<HashKind>(AllHashes.Length);
        foreach (var hashSetting in hashSettings)
        {
            if (hashSet.Add(hashSetting.Kind))
            {
                yield return hashSetting!;
            }
        }
        foreach (var hash in AllHashes)
        {
            if (hashSet.Add(hash))
            {
                yield return CreateHashSetting(hash)!;
            }
        }
    }

    private HashSetting CreateHashSetting(HashKind hash)
    {
        return new HashSetting()
        {
            Kind = hash,
            IsChecked = false,
            IsEnabledForApp = _hashes[hash].IsEnabledForApp,
            IsEnabledForContextMenu = _hashes[hash].IsEnabledForContextMenu,
        };
    }
}
