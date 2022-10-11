using System.Reflection;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IAppearanceSettingsService _appearanceSettingsService;

    [ObservableProperty]
    private string _versionDescription;

    #region AlwaysOnTop

    [ObservableProperty]
    private bool _isAlwaysOnTop;

    partial void OnIsAlwaysOnTopChanged(bool value) =>
        _appearanceSettingsService.IsAlwaysOnTop = value;

    #endregion AlwaysOnTop

    #region Language

    public AppLanguage[] AppLanguages => _appearanceSettingsService.Languages;

    [ObservableProperty]
    private AppLanguage _appLanguage;

    partial void OnAppLanguageChanged(AppLanguage value) =>
       _appearanceSettingsService.Language = value;

    #endregion Language

    #region Theme

    public record AppTheme(string Name, ElementTheme Theme);

    public IList<AppTheme> Themes { get; } = new AppTheme[]
        {
            new(Localization.Settings_Theme_Default, ElementTheme.Default),
            new(Localization.Settings_Theme_Light, ElementTheme.Light),
            new(Localization.Settings_Theme_Dark, ElementTheme.Dark),
        };

    [ObservableProperty]
    private AppTheme _theme;

    partial void OnThemeChanged(AppTheme value) =>
        _appearanceSettingsService.Theme = value.Theme;

    #endregion Theme

    #region HashFontFamily

    [ObservableProperty]
    private string _hashFontFamilyName;

    partial void OnHashFontFamilyNameChanged(string value) =>
        _appearanceSettingsService.HashFontFamilyName = value;

    #endregion HashFontFamily

    public SettingsViewModel(
        INavigationService navigationService,
        IAppearanceSettingsService appearanceSettingsService)
    {
        _navigationService = navigationService;
        _appearanceSettingsService = appearanceSettingsService;

        _isAlwaysOnTop = _appearanceSettingsService.IsAlwaysOnTop;

        _appLanguage = _appearanceSettingsService.Language;

        _theme = Themes.First(x => x.Theme == _appearanceSettingsService.Theme);

        _hashFontFamilyName = _appearanceSettingsService.HashFontFamilyName;

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

    public void RestartApp()
    {
        Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
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
