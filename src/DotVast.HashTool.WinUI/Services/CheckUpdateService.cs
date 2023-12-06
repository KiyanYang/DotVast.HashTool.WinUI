// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Net.Http.Json;
using System.Text.RegularExpressions;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Services;

internal sealed partial class CheckUpdateService(
    IHttpClientFactory httpClientFactory,
    ILogger<CheckUpdateService> logger,
    IDialogService dialogService,
    IPreferencesSettingsService preferencesSettingsService) : ICheckUpdateService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<CheckUpdateService> _logger = logger;
    private readonly IDialogService _dialogService = dialogService;
    private readonly IPreferencesSettingsService _preferencesSettingsService = preferencesSettingsService;

    public Task StartupAsync()
    {
        _ = CheckForUpdateOnStartupAsync();
        return Task.CompletedTask;
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
            // TODO: Notification
            // TODO: System.Net.Http.HttpRequestException: Response status code does not indicate success: 403 (rate limit exceeded).
        }

        if (_gitHubRelease != null)
        {
            _gitHubRelease.Description = DescriptionToExcludeRegex().Replace(_gitHubRelease.Description, string.Empty);
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

    [GeneratedRegex(@"(\s[0-9A-Fa-f]{40}|###.+校验[\S\s]+$)", RegexOptions.ExplicitCapture)]
    private static partial Regex DescriptionToExcludeRegex();
}
