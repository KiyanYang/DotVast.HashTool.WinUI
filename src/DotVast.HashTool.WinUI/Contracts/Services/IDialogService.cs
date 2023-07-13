// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IDialogService
{
    Task<ContentDialogResult> ShowDialogAsync(string title, string content, string closeButtonText, string primaryButtonText = "", string secondaryButtonText = "");

    Task<ContentDialogResult> ShowGithubUpdateDialogAsync(GitHubRelease release);

    Task<ContentDialogResult> ShowHashResultConfigDialogAsync(IList<HashOption> hashOptions);
}
