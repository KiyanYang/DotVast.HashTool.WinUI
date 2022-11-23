using System.Net.Http.Json;
using System.Text.RegularExpressions;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Services;

internal partial class CheckUpdateService : ICheckUpdateService
{
    private readonly HttpClient _client;
    private readonly ILogger<CheckUpdateService> _logger;

    public CheckUpdateService(
        IHttpClientFactory httpClientFactory,
        ILogger<CheckUpdateService> logger)
    {
        _client = httpClientFactory.CreateClient();
        _logger = logger;

        _client.BaseAddress = new(Constants.GitHubRestApi.BaseUrl);
        _client.Timeout = TimeSpan.FromSeconds(5);

        // https://docs.github.com/en/rest/overview/media-types
        // https://docs.github.com/en/rest/overview/resources-in-the-rest-api#user-agent-required
        _client.DefaultRequestHeaders.Accept.Add(new("application/vnd.github+json"));
        _client.DefaultRequestHeaders.UserAgent.Add(new("DotVast.HashTool.WinUI", Package.Current.Id.Version.ToString()));
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
            const string GitHashPattern = @"\s[0-9A-Fa-f]{40}";
            const string VerifyingHashPattern = @"###.+校验[\S\s]+$";
            _gitHubRelease.Description = Regex.Replace(_gitHubRelease.Description, GitHashPattern, string.Empty);
            _gitHubRelease.Description = Regex.Replace(_gitHubRelease.Description, VerifyingHashPattern, string.Empty);
        }

        return _gitHubRelease;
    }
}
