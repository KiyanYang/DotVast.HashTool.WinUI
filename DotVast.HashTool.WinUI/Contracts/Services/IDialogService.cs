using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IDialogService
{
    Task<ContentDialogResult> ShowInfoDialogAsync(string title, string closeButtonText);

    Task<ContentDialogResult> ShowInfoDialogAsync(string title, string content, string closeButtonText);

    Task<ContentDialogResult> ShowGithubUpdateDialogAsync(GitHubRelease release);
}
