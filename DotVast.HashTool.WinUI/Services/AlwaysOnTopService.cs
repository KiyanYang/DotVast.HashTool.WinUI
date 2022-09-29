using DotVast.HashTool.WinUI.Contracts.Services;

namespace DotVast.HashTool.WinUI.Services;

internal class AlwaysOnTopService : IAlwaysOnTopService
{
    private const string SettingsKey = "AppIsAlwaysOnTop";

    public bool IsAlwaysOnTop { get; set; } = false;

    private readonly ILocalSettingsService _localSettingsService;

    public AlwaysOnTopService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        IsAlwaysOnTop = await LoadIsAlwaysOnTopFromSettingsAsync();
    }

    public async Task SetIsAlwaysOnTopAsync(bool isAlwaysOnTop)
    {
        IsAlwaysOnTop = isAlwaysOnTop;
        await SetRequestedIsAlwaysOnTopAsync();
        await SaveIsAlwaysOnTopInSettingsAsync(isAlwaysOnTop);
    }

    public async Task SetRequestedIsAlwaysOnTopAsync()
    {
        App.MainWindow.IsAlwaysOnTop = IsAlwaysOnTop;
        await Task.CompletedTask;
    }

    private async Task<bool> LoadIsAlwaysOnTopFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<bool>(SettingsKey);
    }

    private async Task SaveIsAlwaysOnTopInSettingsAsync(bool isAlwaysOnTop)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, isAlwaysOnTop);
    }
}
