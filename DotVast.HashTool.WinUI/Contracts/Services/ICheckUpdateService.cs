using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ICheckUpdateService
{
    Task<GitHubRelease?> GetLatestGitHubReleaseAsync(bool includePreRelease = false);
}
