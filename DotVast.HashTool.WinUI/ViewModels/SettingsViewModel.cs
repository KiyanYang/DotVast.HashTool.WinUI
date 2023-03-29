using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers;

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
            Title = BaseLocalization.GetLocalized(Localization.SubtreeId, nameof(Localization.RestartToApplyChange), value.Tag),
            Duration = TimeSpan.FromMilliseconds(3000),
            Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational,
        });
    }

    #endregion Language

    #region Theme

    public AppTheme[] Themes { get; } = GenericEnum.GetFieldValues<AppTheme>();

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

    #region FileExplorerContextMenusEnabled

    [ObservableProperty]
    private bool _fileExplorerContextMenusEnabled;

    partial void OnFileExplorerContextMenusEnabledChanged(bool value) =>
        _preferencesSettingsService.FileExplorerContextMenusEnabled = value;

    #endregion FileExplorerContextMenusEnabled

    #region IncludePreRelease

    [ObservableProperty]
    private bool _includePreRelease;

    partial void OnIncludePreReleaseChanged(bool value) =>
        _preferencesSettingsService.IncludePreRelease = value;

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

        _theme = Themes.First(x => x.Theme == _appearanceSettingsService.Theme);

        _hashFontFamilyName = _appearanceSettingsService.HashFontFamilyName;

        _fileExplorerContextMenusEnabled = _preferencesSettingsService.FileExplorerContextMenusEnabled;

        _includePreRelease = _preferencesSettingsService.IncludePreRelease;

        _checkForUpdatesOnStartup = _preferencesSettingsService.CheckForUpdatesOnStartup;

#if DEBUG
        AppVersionHeader = $"{Localization.AppDisplayNameDev}  {RuntimeHelper.AppVersion}";
#elif GITHUB_ACTIONS
        var assemblyInformationalVersion = typeof(SettingsViewModel).Assembly
            .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), true)
            .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
            .First().InformationalVersion;
        AppVersionHeader = $"{Localization.AppDisplayName}  {assemblyInformationalVersion}";
#else
        AppVersionHeader = $"{Localization.AppDisplayName}  {RuntimeHelper.AppVersion}";
#endif
    }

    [RelayCommand]
    private void NavigateTo(string pageKey) => _navigationService.NavigateTo(pageKey);

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

    [ObservableProperty]
    private bool _isCheckingUpdate;

    private bool _isCheckedUpdate = false;

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        IsCheckingUpdate = true;

        // 首次请求需要预热, 用时 1s 左右, 后续用时 100ms - 500ms(网络状况差时), 因此增加一个延时
        if (_isCheckedUpdate)
        {
            await Task.Delay(500);
        }
        else
        {
            _isCheckedUpdate = true;
        }

        var release = await _checkUpdateService.GetLatestGitHubReleaseAsync(IncludePreRelease);

        IsCheckingUpdate = false;

        if (release is null)
        {
            await _dialogService.ShowDialogAsync(
                LocalizationDialog.GitHubUpdate_Title_Failed,
                LocalizationDialog.GitHubUpdate_Content_CheckNetwork,
                LocalizationCommon.Close);
            return;
        }

        if (release.Version > RuntimeHelper.AppVersion)
        {
            await _dialogService.ShowGithubUpdateDialogAsync(release);
        }
        else
        {
            await _dialogService.ShowDialogAsync(
                LocalizationDialog.GitHubUpdate_Title_UpToDate,
                string.Empty,
                LocalizationCommon.Close);
        }
    }
}
