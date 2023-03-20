using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Core.Helpers;

using Windows.Storage;

namespace DotVast.HashTool.WinUI.Services;

public sealed class LocalSettingsService : ILocalSettingsService
{
    public async Task<(bool HasValue, T? Value)> ReadSettingAsync<T>(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
        {
            return (true, await Json.ToObjectAsync<T>((string)obj));
        }
        return (false, default);
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
    }
}
