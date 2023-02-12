namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ILocalSettingsService
{
    Task<(bool Has, T? Val)> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);
}
