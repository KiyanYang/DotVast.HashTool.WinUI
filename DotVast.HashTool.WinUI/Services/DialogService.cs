using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.UserControls.Dialogs;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DotVast.HashTool.WinUI.Services;

internal class DialogService : IDialogService
{
    public async Task<ContentDialogResult> ShowInfoDialogAsync(string title, string closeButtonText)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            CloseButtonText = closeButtonText,
        };

        SetupDialog(dialog);

        return await dialog.ShowAsync();
    }

    public async Task<ContentDialogResult> ShowInfoDialogAsync(string title, string content, string closeButtonText)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            CloseButtonText = closeButtonText,
        };

        SetupDialog(dialog);

        return await dialog.ShowAsync();
    }

    public async Task<ContentDialogResult> ShowGithubUpdateDialogAsync(GitHubRelease release)
    {
        var dialog = new GithubUpdateDialog
        {
            Release = release,
            PrimaryButtonText = Localization.Dialog_GitHubUpdate_Download,
            CloseButtonText = Localization.Dialog_Base_Close,
        };

        SetupDialog(dialog);

        return await dialog.ShowAsync();
    }

    private static void SetupDialog(ContentDialog dialog)
    {
        dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
        Interaction.SetBehaviors(dialog, new() { new ContentDialogBehavior() });
    }
}
