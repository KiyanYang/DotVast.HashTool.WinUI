using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal class HashOptionsService : IHashOptionsService
{
    private const string SettingsKeyPrefix = "HashOption_";

    private readonly ILocalSettingsService _localSettingsService;

    public List<HashOption> HashOptions { get; } = new();

    public HashOptionsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        var hashes = new[]
        {
            Hash.CRC32,

            Hash.MD4,
            Hash.MD5,

            Hash.SHA1,
            Hash.SHA224,
            Hash.SHA256,
            Hash.SHA384,
            Hash.SHA512,
            Hash.SHA3_224,
            Hash.SHA3_256,
            Hash.SHA3_384 ,
            Hash.SHA3_512,

            Hash.Blake2B_160,
            Hash.Blake2B_256,
            Hash.Blake2B_384,
            Hash.Blake2B_512,
            Hash.Blake2S_128,
            Hash.Blake2S_160,
            Hash.Blake2S_224,
            Hash.Blake2S_256,

            Hash.Keccak_224,
            Hash.Keccak_256,
            Hash.Keccak_288,
            Hash.Keccak_384,
            Hash.Keccak_512,

            Hash.QuickXor,
        };
        foreach (var hash in hashes)
        {
            HashOptions.Add(await LoadHashOptionFromSettingsAsync(hash));
        }
    }

    public async Task SetHashOptionAsync(HashOption hashOption)
    {
        await SaveHashOptionAsync(hashOption);
    }

    private async Task<HashOption> LoadHashOptionFromSettingsAsync(Hash hash)
    {
        var hashOption = await _localSettingsService.ReadSettingAsync<HashOption>($"{SettingsKeyPrefix}{hash.Name}");
        if (hashOption == null)
        {
            return new(hash);
        }
        else
        {
            hashOption.Hash = hash;
            return hashOption;
        }
    }

    private async Task SaveHashOptionAsync(HashOption hashOption)
    {
        await _localSettingsService.SaveSettingAsync($"{SettingsKeyPrefix}{hashOption.Hash.Name}", hashOption);
    }
}
