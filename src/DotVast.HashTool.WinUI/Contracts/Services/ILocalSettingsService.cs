// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ILocalSettingsService
{
    [return: NotNullIfNotNull(nameof(defaultValue))]
    T? ReadSetting<T>(string key, T? defaultValue = default);

    [return: NotNullIfNotNull(nameof(defaultValue))]
    T? ReadSetting<T>(string containerName, string key, T? defaultValue = default);

    void SaveSetting<T>(string key, T value);

    void SaveSetting<T>(string containerName, string key, T value);
}
