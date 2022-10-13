using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IAppearanceSettingsService _appearanceSettingsService;
    private readonly ICheckUpdateService _checkUpdateService;
    private readonly IDialogService _dialogService;

    public string AppVersionHeader { get; }
    public string AppVersionDescription { get; }

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
        IAppearanceSettingsService appearanceSettingsService,
        ICheckUpdateService checkUpdateService,
        IDialogService dialogService)
    {
        _navigationService = navigationService;
        _appearanceSettingsService = appearanceSettingsService;
        _checkUpdateService = checkUpdateService;
        _dialogService = dialogService;

        _isAlwaysOnTop = _appearanceSettingsService.IsAlwaysOnTop;

        _appLanguage = _appearanceSettingsService.Language;

        _theme = Themes.First(x => x.Theme == _appearanceSettingsService.Theme);

        _hashFontFamilyName = _appearanceSettingsService.HashFontFamilyName;

#if DEBUG
        AppVersionHeader = $"{Localization.AppDisplayNameDev} - {RuntimeHelper.AppVersion}";
#else
        AppVersionHeader = $"{Localization.AppDisplayName} - {RuntimeHelper.AppVersion}";
#endif
        AppVersionDescription = Localization.SettingsPage_AppDescription;
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

    [RelayCommand]
    private void RestartApp()
    {
        Microsoft.Windows.AppLifecycle.AppInstance.Restart(string.Empty);
    }

    [ObservableProperty]
    private bool _isCheckUpdateProgressActive;

    private bool _isUpdateChecked = false;

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        IsCheckUpdateProgressActive = true;

        // 首次请求需要预热, 用时 1s 左右, 后续用时 100ms - 500ms(网络状况差时), 因此增加一个延时
        if (_isUpdateChecked)
        {
            await Task.Delay(500);
        }
        else
        {
            _isUpdateChecked = true;
        }

        // TODO: 增加“是否检测预览版”设置
        var release = await _checkUpdateService.GetLatestGitHubReleaseAsync(true);

        IsCheckUpdateProgressActive = false;

        if (release == null)
        {
            await _dialogService.ShowInfoDialogAsync(
                Localization.Dialog_GitHubUpdate_Failed,
                Localization.Dialog_GitHubUpdate_CheckNetwork,
                Localization.Dialog_Base_Close);
            return;
        }

        if (release.Version > RuntimeHelper.AppVersion)
        {
            await _dialogService.ShowGithubUpdateDialogAsync(release);
        }
        else
        {
            await _dialogService.ShowInfoDialogAsync(
                Localization.Dialog_GitHubUpdate_UpToDate,
                Localization.Dialog_Base_Close);
        }
    }
}
