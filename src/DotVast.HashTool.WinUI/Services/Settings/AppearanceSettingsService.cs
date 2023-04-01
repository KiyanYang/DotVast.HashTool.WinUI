using System.Diagnostics;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using Windows.Globalization;

using static DotVast.HashTool.WinUI.Constants;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class AppearanceSettingsService : BaseObservableSettings, IAppearanceSettingsService
{
    public override async Task InitializeAsync()
    {
        _hashFontFamilyName = await LoadAsync(nameof(HashFontFamilyName), DefaultAppearanceSettings.HashFontFamilyName);
        _isAlwaysOnTop = await LoadAsync(nameof(IsAlwaysOnTop), DefaultAppearanceSettings.IsAlwaysOnTop);
        _theme = await LoadAsync(nameof(Theme), DefaultAppearanceSettings.Theme);
        Languages = GenericEnum.GetFieldValues<AppLanguage>();
        Language = Languages.Where(x => x.Tag == ApplicationLanguages.PrimaryLanguageOverride)
                            .FirstOrDefault() ?? AppLanguage.ZhHans;

        SetTheme(); // 在初始化时就设置主题
    }

    public override async Task StartupAsync()
    {
        SetIsAlwaysOnTop();
        SetLanguage();

        Debug.Assert((App.MainWindow.Content as FrameworkElement)?.RequestedTheme == Theme);
        Debug.Assert(App.MainWindow.IsAlwaysOnTop == IsAlwaysOnTop);
        Debug.Assert(ApplicationLanguages.PrimaryLanguageOverride == Language.Tag);

        await Task.CompletedTask;
    }

    #region HashFontFamilyName
    private string _hashFontFamilyName = string.Empty;
    public string HashFontFamilyName
    {
        get => _hashFontFamilyName;
        set => SetAndSave(ref _hashFontFamilyName, value);
    }
    #endregion HashFontFamilyName

    #region IsAlwaysOnTop
    private bool _isAlwaysOnTop;
    public bool IsAlwaysOnTop
    {
        get => _isAlwaysOnTop;
        set => SetAndSave(ref _isAlwaysOnTop, value, SetIsAlwaysOnTop);
    }
    private void SetIsAlwaysOnTop()
    {
        App.MainWindow.IsAlwaysOnTop = IsAlwaysOnTop;
    }
    #endregion IsAlwaysOnTop

    #region Theme
    private ElementTheme _theme;
    public ElementTheme Theme
    {
        get => _theme;
        set => SetAndSave(ref _theme, value, SetTheme);
    }

    private void SetTheme()
    {
        //TODO: 等待 Application.Current.RequestedTheme 的动态更改功能, https://github.com/microsoft/microsoft-ui-xaml/issues/4474
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;
        }

        TitleBarContextMenuHelper.UpdateTitleBarContextMenu(Theme);
    }
    #endregion Theme

    #region Language
    public AppLanguage[] Languages { get; private set; } = Array.Empty<AppLanguage>();

    [ObservableProperty]
    private AppLanguage _language = AppLanguage.ZhHans;

    partial void OnLanguageChanged(AppLanguage value) =>
        SetLanguage();

    private void SetLanguage()
    {
        ApplicationLanguages.PrimaryLanguageOverride = Language.Tag;
    }
    #endregion Language
}
