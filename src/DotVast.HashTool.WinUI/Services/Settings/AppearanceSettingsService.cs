// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics;

using DotVast.HashTool.WinUI.Constants;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using WGAL = Windows.Globalization.ApplicationLanguages;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class AppearanceSettingsService : BaseObservableSettings, IAppearanceSettingsService
{
    public override async Task InitializeAsync()
    {
        _hashFontFamilyName = Load(nameof(HashFontFamilyName), DefaultAppearanceSettings.HashFontFamilyName);
        _isAlwaysOnTop = Load(nameof(IsAlwaysOnTop), DefaultAppearanceSettings.IsAlwaysOnTop);
        _theme = Load(nameof(Theme), DefaultAppearanceSettings.Theme);

        await InitializeThemeAsync();
    }

    public override Task StartupAsync()
    {
        SetIsAlwaysOnTop();

        Debug.Assert((App.MainWindow.Content as FrameworkElement)?.RequestedTheme == Theme.ToElementTheme());
        Debug.Assert(App.MainWindow.IsAlwaysOnTop == IsAlwaysOnTop);
        Debug.Assert(WGAL.PrimaryLanguageOverride == Language.ToTag());

        return Task.CompletedTask;
    }

    #region HashFontFamilyName
    private string _hashFontFamilyName = string.Empty;
    public string HashFontFamilyName
    {
        get => _hashFontFamilyName;
        set => SetPropertyAndSave(value, ref _hashFontFamilyName);
    }
    #endregion HashFontFamilyName

    #region IsAlwaysOnTop
    private bool _isAlwaysOnTop;
    public bool IsAlwaysOnTop
    {
        get => _isAlwaysOnTop;
        set => SetPropertyAndSave(value, ref _isAlwaysOnTop, SetIsAlwaysOnTop);
    }
    private void SetIsAlwaysOnTop()
    {
        App.MainWindow.IsAlwaysOnTop = IsAlwaysOnTop;
    }
    #endregion IsAlwaysOnTop

    #region Theme
    private AppTheme _theme;
    public AppTheme Theme
    {
        get => _theme;
        set => SetPropertyAndSave(value, ref _theme, SetTheme);
    }
    private void SetTheme()
    {
        //TODO: 等待 Application.Current.RequestedTheme 的动态更改功能, https://github.com/microsoft/microsoft-ui-xaml/issues/4474
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme.ToElementTheme();
        }

        TitleBarContextMenuHelper.UpdateTitleBarContextMenu(Theme);
    }
    private async Task InitializeThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            while (!rootElement.IsLoaded)
            {
                await Task.Delay(25);
            }
        }
        SetTheme();
    }
    #endregion Theme

    #region Language
    [ObservableProperty]
    private AppLanguage _language = WGAL.PrimaryLanguageOverride.ToAppLanguage();

    partial void OnLanguageChanged(AppLanguage value) =>
        SetLanguage();

    private void SetLanguage()
    {
        WGAL.PrimaryLanguageOverride = Language.ToTag();
    }
    #endregion Language
}
