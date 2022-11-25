using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI;

internal static class Constants
{
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
        public const bool IncludePreRelease = false;
    }

    public static class LocalSettingsOptions
    {
        public const string ApplicationDataFolder = "DotVast.HashTool.WinUI/ApplicationData";
        public const string LocalSettingsFile = "LocalSettings.json";
    }

    public static class LogsOptions
    {
        public const string FilePath = "Logs/app-.log";
    }
}
