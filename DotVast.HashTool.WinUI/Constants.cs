using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI;

internal static class Constants
{
    public static class CommandLineArgs
    {
        public const string Hash = "--hash";
        public const string Path = "--path";
    }

    public static class PageKeys
    {
        private static readonly Type s_homePageType = typeof(HomeViewModel);
        private static readonly Type s_taskPageType = typeof(TasksViewModel);
        private static readonly Type s_resultsPageType = typeof(ResultsViewModel);
        private static readonly Type s_settingsPageType = typeof(SettingsViewModel);
        private static readonly Type s_hashOptionSettingsPageType = typeof(HashOptionSettingsViewModel);
        public static readonly string HomePage = s_homePageType.FullName!;
        public static readonly string TaskPage = s_taskPageType.FullName!;
        public static readonly string ResultsPage = s_resultsPageType.FullName!;
        public static readonly string SettingsPage = s_settingsPageType.FullName!;
        public static readonly string HashOptionSettingsPageType = s_hashOptionSettingsPageType.FullName!;
    }

    public static class GitHubRestApi
    {
        public const string BaseUrl = "https://api.github.com/";
        public const string LatestReleaseUrl = "/repos/KiyanYang/DotVast.HashTool.WinUI/releases/latest";
        public const string ReleasesUrl = "/repos/KiyanYang/DotVast.HashTool.WinUI/releases?per_page=1&page=1";
    }

    public static class DefaultAppearanceSettings
    {
        public const string HashFontFamilyName = "Consolas";
        public const bool IsAlwaysOnTop = false;
        public const ElementTheme Theme = ElementTheme.Default;
    }

    public static class DefaultPreferencesSettings
    {
        public const bool FileExplorerContextMenusEnabled = true;
        public const bool IncludePreRelease = false;
        public const bool CheckForUpdatesOnStartup = false;
        public const bool StartingWhenCreateHashTask = true;
    }

    public static class LogsOptions
    {
        public const string FilePath = "Logs/app-.log";
    }

    public static class HttpClient
    {
        public const string GitHubRestApi = nameof(GitHubRestApi);
    }
}
