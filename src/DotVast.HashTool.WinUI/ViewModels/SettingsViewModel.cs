using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject, IViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAppearanceSettingsService _appearanceSettingsService;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly ICheckUpdateService _checkUpdateService;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;

    public string AppVersionHeader { get; }

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

    partial void OnAppLanguageChanged(AppLanguage value)
    {
        _appearanceSettingsService.Language = value;
        _notificationService.Show(new()
        {
            Title = Strings.Base.BaseLocalization.GetLocalized(Localization.SubtreeId, nameof(Localization.RestartToApplyChange), value.ToTag()),
            Duration = TimeSpan.FromMilliseconds(3000),
            Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational,
        });
    }

    #endregion Language

    #region Theme

    public AppTheme[] Themes { get; } = Enum.GetValues<AppTheme>();

    [ObservableProperty]
    private AppTheme _theme;

    partial void OnThemeChanged(AppTheme value) =>
        _appearanceSettingsService.Theme = value;

    #endregion Theme

    #region HashFontFamily

    [ObservableProperty]
    private string _hashFontFamilyName;

    partial void OnHashFontFamilyNameChanged(string value) =>
        _appearanceSettingsService.HashFontFamilyName = value;

    #endregion HashFontFamily

    #region FileExplorerContextMenusEnabled

    [ObservableProperty]
    private bool _fileExplorerContextMenusEnabled;

    partial void OnFileExplorerContextMenusEnabledChanged(bool value) =>
        _preferencesSettingsService.FileExplorerContextMenusEnabled = value;

    #endregion FileExplorerContextMenusEnabled

    #region IncludePreRelease

    [ObservableProperty]
    private bool _includePreRelease;

    partial void OnIncludePreReleaseChanged(bool value)
    {
        _preferencesSettingsService.IncludePreRelease = value;

        // 更新该选项时, 刷新 _lastCheckTime 以确保可以获取更新
        _lastCheckTime = new(0);
    }

    #endregion IncludePreRelease

    #region CheckForUpdatesOnStartup

    [ObservableProperty]
    private bool _checkForUpdatesOnStartup;

    partial void OnCheckForUpdatesOnStartupChanged(bool value) =>
        _preferencesSettingsService.CheckForUpdatesOnStartup = value;

    #endregion CheckForUpdatesOnStartup

    public SettingsViewModel(
        INavigationService navigationService,
        IAppearanceSettingsService appearanceSettingsService,
        IPreferencesSettingsService preferencesSettingsService,
        ICheckUpdateService checkUpdateService,
        IDialogService dialogService,
        INotificationService notificationService)
    {
        _navigationService = navigationService;
        _appearanceSettingsService = appearanceSettingsService;
        _preferencesSettingsService = preferencesSettingsService;
        _checkUpdateService = checkUpdateService;
        _dialogService = dialogService;
        _notificationService = notificationService;

        _isAlwaysOnTop = _appearanceSettingsService.IsAlwaysOnTop;

        _appLanguage = _appearanceSettingsService.Language;

        _theme = _appearanceSettingsService.Theme;

        _hashFontFamilyName = _appearanceSettingsService.HashFontFamilyName;

        _fileExplorerContextMenusEnabled = _preferencesSettingsService.FileExplorerContextMenusEnabled;

        _includePreRelease = _preferencesSettingsService.IncludePreRelease;

        _checkForUpdatesOnStartup = _preferencesSettingsService.CheckForUpdatesOnStartup;

#if GITHUB_ACTIONS && !DotVast_CIRelease
        var assemblyInformationalVersion = typeof(SettingsViewModel).Assembly
            .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), true)
            .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
            .First().InformationalVersion;
        AppVersionHeader = $"{Localization.AppDisplayName}  {assemblyInformationalVersion}";
#elif !DEBUG
        AppVersionHeader = $"{Localization.AppDisplayName}  {RuntimeHelper.AppVersion}";
#else
        AppVersionHeader = $"{Localization.AppDisplayNameDev}  {RuntimeHelper.AppVersion}";
#endif
    }

    [RelayCommand]
    private void NavigateTo(PageKey pageKey) => _navigationService.NavigateTo(pageKey);

    [RelayCommand]
    private static async Task NavigateToLogsFolderAsync()
    {
        var localAppData = PathHelper.AppDataLocalPhysicalPath;
        var logsFilePath = Constants.LogsOptions.FilePath;
        var logsFolderPath = Path.Combine(localAppData, Path.GetDirectoryName(logsFilePath) ?? string.Empty);
        await Windows.System.Launcher.LaunchFolderPathAsync(logsFolderPath);
    }

    [RelayCommand]
    private static void RestartApp()
    {
        Microsoft.Windows.AppLifecycle.AppInstance.Restart(string.Empty);
    }

    private DateTime _lastCheckTime = new(0);
    private GitHubRelease? _gitHubRelease;

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        // 若成功获取, 则缓存结果, 缓存 10 分钟内有效, 超时则重新获取.
        if (DateTime.Now - _lastCheckTime > TimeSpan.FromMinutes(10))
        {
            _gitHubRelease = await _checkUpdateService.GetLatestGitHubReleaseAsync(IncludePreRelease);
            if (_gitHubRelease is not null)
            {
                _lastCheckTime = DateTime.Now;
            }
        }

        if (_gitHubRelease is null)
        {
            await _dialogService.ShowDialogAsync(
                LocalizationPopup.GitHubUpdate_Title_Failed,
                LocalizationPopup.GitHubUpdate_Content_CheckNetwork,
                LocalizationCommon.Close);
        }
        else if (_gitHubRelease.Version > RuntimeHelper.AppVersion)
        {
            await _dialogService.ShowGithubUpdateDialogAsync(_gitHubRelease);
        }
        else
        {
            await _dialogService.ShowDialogAsync(
                LocalizationPopup.GitHubUpdate_Title_UpToDate,
                string.Empty,
                LocalizationCommon.Close);
        }
    }
}
