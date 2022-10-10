using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Services.Settings;

internal sealed partial class AppearanceSettingsService : BaseObservableSettings, IAppearanceSettingsService
{
    public async override Task InitializeAsync()
    {
        _hashFontFamilyName = await LoadAsync(nameof(HashFontFamilyName), "Consolas");
        _isAlwaysOnTop = await LoadAsync(nameof(IsAlwaysOnTop), false);
        _theme = await LoadAsync(nameof(Theme), ElementTheme.Default);
        Languages = GenericEnum.GetFieldValues<AppLanguage>();
        _language = Languages.Where(x => x.Tag == ApplicationLanguages.PrimaryLanguageOverride)
                            .FirstOrDefault() ?? AppLanguage.ZhHans;
    }

    public async override Task StartupAsync()
    {
        SetIsAlwaysOnTop();
        SetTheme();
        SetLanguage();
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
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
            //TitleBarContextMenuHelper.UpdateTitleBarContextMenu(Theme);
        }
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
        ApplicationLanguages.PrimaryLanguageOverride = _language.Tag;
    }
    #endregion Language
}
