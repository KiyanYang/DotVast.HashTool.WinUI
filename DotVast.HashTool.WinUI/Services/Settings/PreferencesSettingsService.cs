using System.Collections.ObjectModel;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class PreferencesSettingsService : BaseObservableSettings, IPreferencesSettingsService
{
    public async override Task InitializeAsync()
    {
        await InitializeHashOptions();
    }

    public async override Task StartupAsync()
    {
        await Task.CompletedTask;
    }

    public ObservableCollection<HashOption> HashOptions { get; } = new();

    private async Task InitializeHashOptions()
    {
        var allHashes = GenericEnum.GetFieldValues<Hash>();
        var allHashOptions = allHashes.Select(i => new HashOption(i));

        // 反序列化时, HashOption 的属性 Hash 可能为 null
        var hashOptions = (await LoadAsync<List<HashOption>>(nameof(HashOptions)) ?? new())
            .Where(i => i.Hash != null)
            .UnionBy(allHashOptions, hashOption => hashOption.Hash);

        foreach (var hashOption in hashOptions)
        {
            HashOptions.Add(hashOption);
        }
    }

    public async Task SaveHashOptionsAsync()
    {
        await SaveAsync(HashOptions, nameof(HashOptions));
    }
}
