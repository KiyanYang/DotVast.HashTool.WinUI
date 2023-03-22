using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal abstract partial class BaseObservableSettings : ObservableObject, IBaseObservableSettings
{
    public abstract Task InitializeAsync();
    public abstract Task StartupAsync();

    private readonly ILocalSettingsService _localSettingsService = App.GetService<ILocalSettingsService>();

    protected async Task<T> LoadAsync<T>(string key, T defaultValue)
    {
        var (hasValue, value) = await _localSettingsService.ReadSettingAsync<T>(key);
        return hasValue ? (value ?? defaultValue) : defaultValue;
    }

    protected async Task SaveAsync<T>(T value, string key) =>
        await _localSettingsService.SaveSettingAsync(key, value);

    protected void Save<T>(T value, [CallerMemberName] string key = "") =>
        Task.Run(() => _localSettingsService.SaveSettingAsync(key, value));

    protected bool SetAndSave<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, newValue, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(propertyName, newValue));
            return true;
        }
        return false;
    }

    protected bool SetAndSave<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Action action, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, newValue, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(propertyName, newValue));
            action();
            return true;
        }
        return false;
    }

    protected bool SetAndSave<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Func<Task> func, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, newValue, propertyName) && propertyName != null)
        {
            Task.Run(() => _localSettingsService.SaveSettingAsync(propertyName, newValue));
            func();
            return true;
        }
        return false;
    }
}
