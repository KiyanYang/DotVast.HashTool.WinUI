using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Windows.Storage;

namespace DotVast.HashTool.WinUI.Services;

public sealed class LocalSettingsService : ILocalSettingsService
{
    private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? ReadSetting<T>(string key, T? defaultValue = default)
    {
        if (_localSettings.Values.TryGetValue(key, out var obj) && obj is string str)
        {
            return JsonSerializer.Deserialize<T>(str) ?? defaultValue;
        }
        return defaultValue;
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? ReadSetting<T>(string containerName, string key, T? defaultValue = default)
    {
        var containers = _localSettings.Containers;
        if (containers.TryGetValue(containerName, out var container)
         && container.Values.TryGetValue(key, out var obj)
         && obj is string str)
        {
            return JsonSerializer.Deserialize<T>(str) ?? defaultValue;
        }
        return defaultValue;
    }

    public void SaveSetting<T>(string key, T value)
    {
        _localSettings.Values[key] = JsonSerializer.Serialize(value);
    }

    public void SaveSetting<T>(string containerName, string key, T value)
    {
        var container = _localSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        container.Values[key] = JsonSerializer.Serialize(value);
    }
}
