using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal abstract partial class BaseObservableSettings : ObservableObject, IBaseObservableSettings
{
    public abstract Task InitializeAsync();
    public abstract Task StartupAsync();

    protected readonly ILocalSettingsService _localSettingsService = App.GetService<ILocalSettingsService>();

    protected async Task<T> LoadAsync<T>(string key, T defaultValue)
    {
        var (hasValue, value) = await _localSettingsService.ReadSettingAsync<T>(key);
        return hasValue ? (value ?? defaultValue) : defaultValue;
    }

    // TODO: 之后重构该方法
    protected T Load<T>(string containerName, string key, T defaultValue)
    {
        var containers = Windows.Storage.ApplicationData.Current.LocalSettings.Containers;
        if (containers.TryGetValue(containerName, out var container) && container.Values.TryGetValue(key, out var obj))
        {
            return JsonSerializer.Deserialize<T>((string)obj) ?? defaultValue;
        }
        return defaultValue;
    }

    protected bool SetAndSave<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, newValue, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(propertyName, newValue));
            return true;
        }
        return false;
    }

    protected bool SetAndSave<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, Action action, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, newValue, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(propertyName, newValue));
            action();
            return true;
        }
        return false;
    }

    protected bool SetAndSave<T>(string containerName, string key, T value, [NotNullIfNotNull(nameof(value))] ref T field, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(containerName, key, value));
            return true;
        }
        return false;
    }
}
