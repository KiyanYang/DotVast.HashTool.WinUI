using System.Reflection;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _versionDescription;

    #region Language selector

    private readonly ILanguageSelectorService _languageSelectorService;

    public AppLanguage[] AppLanguages
    {
        get;
    }

    [ObservableProperty]
    private AppLanguage _appLanguage;

    async partial void OnAppLanguageChanged(AppLanguage value)
    {
        await _languageSelectorService.SetAppLanguageAsync(value);
    }

    #endregion Language selector

    #region Theme selector

    private readonly IThemeSelectorService _themeSelectorService;

    public record AppTheme(string Name, ElementTheme Theme);

    public IList<AppTheme> Themes
    {
        get;
    } = new AppTheme[]
    {
        new("Settings_Theme_Default".GetLocalized(), ElementTheme.Default),
        new("Settings_Theme_Light".GetLocalized(), ElementTheme.Light),
        new("Settings_Theme_Dark".GetLocalized(), ElementTheme.Dark),
    };

    [ObservableProperty]
    private AppTheme _theme;

    async partial void OnThemeChanged(AppTheme value) =>
        await _themeSelectorService.SetThemeAsync(value.Theme);

    #endregion Theme selector

    public SettingsViewModel(IThemeSelectorService themeSelectorService, ILanguageSelectorService languageSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _theme = Themes.First(x => x.Theme == _themeSelectorService.Theme);
        _languageSelectorService = languageSelectorService;
        _appLanguage = _languageSelectorService.Language;
        AppLanguages = _languageSelectorService.Languages;
        _versionDescription = GetVersionDescription();
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
