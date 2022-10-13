using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.UserControls.Dialogs;

public sealed partial class GithubUpdateDialog : ContentDialog
{
    private GitHubRelease? _release;

    public GitHubRelease? Release
    {
        get => _release;
        set => SetData(value);
    }

    public GithubUpdateDialog()
    {
        InitializeComponent();
        DefaultButton = ContentDialogButton.Primary;
        PrimaryButtonClick += GithubUpdateDialog_PrimaryButtonClick;
    }

    private void SetData(GitHubRelease? release)
    {
        if (release == null)
        {
            return;
        }

        _release = release;
        TitleText.Text = release.TagName;
        PublishAtText.Text = release.PublishAt.ToLocalTime().ToString();
        MarkdownText.Text = release.Description;
    }

    private async void GithubUpdateDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (_release == null)
        {
            return;
        }

        await Windows.System.Launcher.LaunchUriAsync(new(_release.Url));
    }
}
