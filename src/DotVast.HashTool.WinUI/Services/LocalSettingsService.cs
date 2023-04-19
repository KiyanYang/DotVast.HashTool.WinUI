// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

public sealed partial class LocalSettingsService : ILocalSettingsService
{
    private readonly Windows.Storage.ApplicationDataContainer _localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? ReadSetting<T>(string key, T? defaultValue = default)
    {
        if (_localSettings.Values.TryGetValue(key, out var obj) && obj is string str)
        {
            return JsonSerializer.Deserialize<T>(str, JsonContextForSettings.Default.GetTypeInfo<T>()) ?? defaultValue;
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
            return JsonSerializer.Deserialize<T>(str, JsonContextForSettings.Default.GetTypeInfo<T>()) ?? defaultValue;
        }
        return defaultValue;
    }

    public void SaveSetting<T>(string key, T value)
    {
        _localSettings.Values[key] = JsonSerializer.Serialize(value, JsonContextForSettings.Default.GetTypeInfo<T>());
    }

    public void SaveSetting<T>(string containerName, string key, T value)
    {
        var container = _localSettings.CreateContainer(containerName, Windows.Storage.ApplicationDataCreateDisposition.Always);
        container.Values[key] = JsonSerializer.Serialize(value, JsonContextForSettings.Default.GetTypeInfo<T>());
    }

    /// <summary>
    /// 用于设置的 JsonSerializerContext.
    /// </summary>
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(AppTheme))]
    [JsonSerializable(typeof(FileAttributes))]
    [JsonSerializable(typeof(HashSetting))]
    private sealed partial class JsonContextForSettings : JsonSerializerContext
    {
        public JsonTypeInfo<T> GetTypeInfo<T>()
        {
            return GetTypeInfo(typeof(T)) as JsonTypeInfo<T> ?? throw new ArgumentOutOfRangeException();
        }
    }
}
