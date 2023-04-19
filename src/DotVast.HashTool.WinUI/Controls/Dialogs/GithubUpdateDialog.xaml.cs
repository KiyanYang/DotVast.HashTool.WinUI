// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Controls.Dialogs;

public sealed partial class GithubUpdateDialog : ContentDialog
{
    private GitHubRelease? _release;

    public GitHubRelease? Release
    {
        get => _release;
        set
        {
            if (value is null)
            {
                return;
            }

            _release = value;
            TitleText.Text = value.TagName;
            PublishAtText.Text = value.PublishAt.ToLocalTime().ToString();
            MarkdownText.Text = value.Description;
        }
    }

    public GithubUpdateDialog()
    {
        InitializeComponent();
        DefaultButton = ContentDialogButton.Primary;
        PrimaryButtonClick += GithubUpdateDialog_PrimaryButtonClick;
    }

    private async void GithubUpdateDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (Release is null)
        {
            return;
        }

        await Windows.System.Launcher.LaunchUriAsync(new(Release.Url));
    }
}
