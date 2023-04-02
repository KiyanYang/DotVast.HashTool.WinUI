namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ILocalSettingsService
{
    Task<(bool HasValue, T? Value)> ReadSettingAsync<T>(string key);

    Task<(bool HasValue, T? Value)> ReadSettingAsync<T>(string containerName, string key);

    Task SaveSettingAsync<T>(string key, T value);

    Task SaveSettingAsync<T>(string containerName, string key, T value);
}
