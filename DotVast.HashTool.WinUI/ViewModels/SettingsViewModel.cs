using System.Reflection;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;

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
        new(Localization.Settings_Theme_Default, ElementTheme.Default),
        new(Localization.Settings_Theme_Light, ElementTheme.Light),
        new(Localization.Settings_Theme_Dark, ElementTheme.Dark),
    };

    [ObservableProperty]
    private AppTheme _theme;

    async partial void OnThemeChanged(AppTheme value) =>
        await _themeSelectorService.SetThemeAsync(value.Theme);

    #endregion Theme selector

    public SettingsViewModel(
        INavigationService navigationService,
        IThemeSelectorService themeSelectorService,
        ILanguageSelectorService languageSelectorService)
    {
        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _theme = Themes.First(x => x.Theme == _themeSelectorService.Theme);
        _languageSelectorService = languageSelectorService;
        _appLanguage = _languageSelectorService.Language;
        AppLanguages = _languageSelectorService.Languages;
        _versionDescription = GetVersionDescription();
    }

    [RelayCommand]
    private void NavigateTo(string pageKey) => _navigationService.NavigateTo(pageKey);

    [RelayCommand]
    private async void NavigateToLogsFolder()
    {
        var localAppData = PathHelper.AppDataLocalPhysicalPath;
        var logsFilePath = App.GetOptions<LogsOptions>().Value.FilePath!;
        var logsFolderPath = Path.GetDirectoryName(Path.Combine(localAppData, logsFilePath));
        await Windows.System.Launcher.LaunchFolderPathAsync(logsFolderPath);
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

        return $"{Localization.AppDisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
