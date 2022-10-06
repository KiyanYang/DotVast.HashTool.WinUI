using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal class HashOptionsService : IHashOptionsService
{
    private const string SettingsKey = "HashOptions";

    private readonly ILocalSettingsService _localSettingsService;

    public ObservableCollection<HashOption> HashOptions { get; } = new();

    public HashOptionsService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        var allHasheOptions = new HashOption[]
        {
            new(Hash.CRC32),

            new(Hash.MD4),
            new(Hash.MD5, isChecked: true),

            new(Hash.SHA1, isChecked:true),
            new(Hash.SHA224),
            new(Hash.SHA256, isChecked:true),
            new(Hash.SHA384),
            new(Hash.SHA512),
            new(Hash.SHA3_224),
            new(Hash.SHA3_256),
            new(Hash.SHA3_384 ),
            new(Hash.SHA3_512),

            new(Hash.SM3),

            new(Hash.Blake2B_160),
            new(Hash.Blake2B_256),
            new(Hash.Blake2B_384),
            new(Hash.Blake2B_512),
            new(Hash.Blake2S_128),
            new(Hash.Blake2S_160),
            new(Hash.Blake2S_224),
            new(Hash.Blake2S_256),
            new(Hash.Blake3),

            new(Hash.RIPEMD128),
            new(Hash.RIPEMD160),
            new(Hash.RIPEMD256),
            new(Hash.RIPEMD320),

            new(Hash.Keccak_224),
            new(Hash.Keccak_256),
            new(Hash.Keccak_288),
            new(Hash.Keccak_384),
            new(Hash.Keccak_512),

            new(Hash.QuickXor),
        };

        // 反序列化时, HashOption 的属性 Hash 可能为 null
        var hashOptions = (await LoadHashOptionsFromSettingsAsync() ?? new())
            .Where(i => i.Hash != null)
            .UnionBy(allHasheOptions, hashOption => hashOption.Hash);

        foreach (var hashOption in hashOptions)
        {
            HashOptions.Add(hashOption);
        }
    }

    public async Task SetHashOptionsAsync(IList<HashOption> hashOptions)
    {
        await SaveHashOptionsAsync(hashOptions);
    }

    private async Task<List<HashOption>?> LoadHashOptionsFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<List<HashOption>>(SettingsKey);
    }

    private async Task SaveHashOptionsAsync(IList<HashOption> hashOptions)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, hashOptions);
    }
}
