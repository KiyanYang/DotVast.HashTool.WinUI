using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Constants;

public static class CommandLineArgs
{
    public const string Hash = "--hash";
    public const string Path = "--path";
}

internal static class SettingsContainerName
{
    public const string ContextMenu = "ContextMenu";
    public const string DataOptions_Hashes = "DataOptions:Hashes";
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
    public const AppTheme Theme = AppTheme.Default;
}

public static class DefaultPreferencesSettings
{
    public const FileAttributes FileAttributesToSkipWhenFolderMode = FileAttributes.Hidden | FileAttributes.Offline | FileAttributes.System;
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
