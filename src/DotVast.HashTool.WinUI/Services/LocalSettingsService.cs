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

    public async Task<(bool HasValue, T? Value)> ReadSettingAsync<T>(string containerName, string key)
    {
        var containers = ApplicationData.Current.LocalSettings.Containers;
        if (containers.TryGetValue(containerName, out var container) && container.Values.TryGetValue(key, out var obj))
        {
            return (true, await Json.ToObjectAsync<T>((string)obj));
        }

        return (false, default);
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
    }

    public async Task SaveSettingAsync<T>(string containerName, string key, T value)
    {
        var container = ApplicationData.Current.LocalSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        container.Values[key] = await Json.StringifyAsync(value);
    }
}
