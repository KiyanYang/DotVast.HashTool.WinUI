using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ICheckUpdateService
{
    /// <summary>
    /// StartupAsync <see cref="IActivationService.ActivateAsync(object)"/>.
    /// </summary>
    /// <returns></returns>
    Task StartupAsync();

    /// <summary>
    /// 获取最新 GitHub 发布信息.
    /// </summary>
    /// <param name="includePreRelease">包括预发布版本.</param>
    /// <returns></returns>
    Task<GitHubRelease?> GetLatestGitHubReleaseAsync(bool includePreRelease = false);
}
