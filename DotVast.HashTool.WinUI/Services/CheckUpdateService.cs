using System.Net.Http.Json;
using System.Text.RegularExpressions;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Services;

internal partial class CheckUpdateService : ICheckUpdateService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CheckUpdateService> _logger;
    private readonly IDialogService _dialogService;
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public CheckUpdateService(
        IHttpClientFactory httpClientFactory,
        ILogger<CheckUpdateService> logger,
        IDialogService dialogService,
        IPreferencesSettingsService preferencesSettingsService)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _dialogService = dialogService;
        _preferencesSettingsService = preferencesSettingsService;
    }

    public async Task StartupAsync()
    {
        await CheckForUpdateOnStartupAsync();
    }

    public async Task<GitHubRelease?> GetLatestGitHubReleaseAsync(bool includePreRelease = false)
    {
        GitHubRelease? _gitHubRelease = null;
        using var client = _httpClientFactory.CreateClient(Constants.HttpClient.GitHubRestApi);

        try
        {
            if (includePreRelease)
            {
                var releases = await client.GetFromJsonAsync(Constants.GitHubRestApi.ReleasesUrl, JsonContext.Default.GitHubReleaseArray);
                _gitHubRelease = releases?.FirstOrDefault();
            }
            else
            {
                _gitHubRelease = await client.GetFromJsonAsync(Constants.GitHubRestApi.LatestReleaseUrl, JsonContext.Default.GitHubRelease);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("检查更新时发生错误\n{Exception}", ex);
        }

        if (_gitHubRelease != null)
        {
            _gitHubRelease.Description = GitCommitHashRegex().Replace(_gitHubRelease.Description, string.Empty);
            _gitHubRelease.Description = VerifyingHashRegex().Replace(_gitHubRelease.Description, string.Empty);
        }

        return _gitHubRelease;
    }

    private async Task CheckForUpdateOnStartupAsync()
    {
        if (!_preferencesSettingsService.CheckForUpdatesOnStartup)
        {
            return;
        }

        var release = await GetLatestGitHubReleaseAsync(_preferencesSettingsService.IncludePreRelease);
        if (release?.Version > RuntimeHelper.AppVersion)
        {
            await _dialogService.ShowGithubUpdateDialogAsync(release);
        }
    }

    [GeneratedRegex(@"\s[0-9A-Fa-f]{40}")]
    private static partial Regex GitCommitHashRegex();

    [GeneratedRegex(@"###.+校验[\S\s]+$")]
    private static partial Regex VerifyingHashRegex();
}
