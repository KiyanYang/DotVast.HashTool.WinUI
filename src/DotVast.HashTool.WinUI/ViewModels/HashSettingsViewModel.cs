// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HashSettingsViewModel(IPreferencesSettingsService preferencesSettingsService) : ObservableObject, IViewModel
{
    private readonly IPreferencesSettingsService _preferencesSettingsService = preferencesSettingsService;

    public IReadOnlyList<HashSetting> HashSettings => _preferencesSettingsService.HashSettings;

    public IReadOnlyList<HashFormat> HashFormats { get; } = Enum.GetValues<HashFormat>();
}
