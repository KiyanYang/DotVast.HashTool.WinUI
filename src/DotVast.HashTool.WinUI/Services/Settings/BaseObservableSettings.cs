// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal abstract partial class BaseObservableSettings : ObservableObject, IBaseObservableSettings
{
    public abstract Task InitializeAsync();
    public abstract Task StartupAsync();

    protected readonly ILocalSettingsService _localSettingsService = App.GetService<ILocalSettingsService>();

    [return: NotNullIfNotNull(nameof(defaultValue))]
    protected T? Load<T>(string key, T? defaultValue = default)
    {
        return _localSettingsService.ReadSetting(key, defaultValue);
    }

    [return: NotNullIfNotNull(nameof(defaultValue))]
    protected T? Load<T>(string containerName, string key, T? defaultValue = default)
    {
        return _localSettingsService.ReadSetting(containerName, key, defaultValue);
    }

    protected bool SetPropertyAndSave<T>(T value, [NotNullIfNotNull(nameof(value))] ref T field, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName) && propertyName != null)
        {
            _localSettingsService.SaveSetting(propertyName, value);
            return true;
        }
        return false;
    }

    protected bool SetPropertyAndSave<T>(string containerName, string key, T value, [NotNullIfNotNull(nameof(value))] ref T field, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName) && propertyName != null)
        {
            _localSettingsService.SaveSetting(containerName, key, value);
            return true;
        }
        return false;
    }

    protected bool SetPropertyAndSave<T>(T value, [NotNullIfNotNull(nameof(value))] ref T field, Action action, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName) && propertyName != null)
        {
            _localSettingsService.SaveSetting(propertyName, value);
            action();
            return true;
        }
        return false;
    }
}
