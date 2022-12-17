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
    private readonly HttpClient _client;
    private readonly ILogger<CheckUpdateService> _logger;
    private readonly IDialogService _dialogService;
    private readonly IPreferencesSettingsService _preferencesSettingsService;

    public CheckUpdateService(
        IHttpClientFactory httpClientFactory,
        ILogger<CheckUpdateService> logger,
        IDialogService dialogService,
        IPreferencesSettingsService preferencesSettingsService)
    {
        _client = httpClientFactory.CreateClient();
        _logger = logger;
        _dialogService = dialogService;
        _preferencesSettingsService = preferencesSettingsService;

        _client.BaseAddress = new(Constants.GitHubRestApi.BaseUrl);
        _client.Timeout = TimeSpan.FromSeconds(5);

        // https://docs.github.com/en/rest/overview/media-types
        // https://docs.github.com/en/rest/overview/resources-in-the-rest-api#user-agent-required
        _client.DefaultRequestHeaders.Accept.Add(new("application/vnd.github+json"));
        _client.DefaultRequestHeaders.UserAgent.Add(new("DotVast.HashTool.WinUI", RuntimeHelper.AppVersion.ToString()));
    }

    public async Task StartupAsync()
    {
        await CheckForUpdateOnStartupAsync();
    }

    public async Task<GitHubRelease?> GetLatestGitHubReleaseAsync(bool includePreRelease = false)
    {
        GitHubRelease? _gitHubRelease = null;
        try
        {
            if (includePreRelease)
            {
                var releases = await _client.GetFromJsonAsync<GitHubRelease[]>(Constants.GitHubRestApi.ReleasesUrl);
                _gitHubRelease = releases?.FirstOrDefault();
            }
            else
            {
                _gitHubRelease = await _client.GetFromJsonAsync<GitHubRelease>(Constants.GitHubRestApi.LatestReleaseUrl);
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
