using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

/// <summary>
/// Defines an interface for a check update service.
/// </summary>
public interface ICheckUpdateService
{
    /// <summary>
    /// StartupAsync <see cref="IActivationService.ActivateAsync(object)"/>.
    /// </summary>
    /// <returns></returns>
    Task StartupAsync();

    /// <summary>
    /// Gets the latest release information from GitHub.
    /// </summary>
    /// <param name="includePreRelease">Whether to include pre-release versions.</param>
    /// <returns>A <see cref="GitHubRelease"/> object or null.</returns>
    Task<GitHubRelease?> GetLatestGitHubReleaseAsync(bool includePreRelease = false);
}
