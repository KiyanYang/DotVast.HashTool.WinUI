using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Controls.Dialogs;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DotVast.HashTool.WinUI.Services;

internal class DialogService : IDialogService
{
    private readonly IAppearanceSettingsService _appearanceSettingsService;

    public DialogService(IAppearanceSettingsService appearanceSettingsService)
    {
        _appearanceSettingsService = appearanceSettingsService;
    }

    public async Task<ContentDialogResult> ShowDialogAsync(
        string title,
        string content,
        string closeButtonText,
        string primaryButtonText = "",
        string secondaryButtonText = "")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
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
            PrimaryButtonText = LocalizationDialog.GitHubUpdate_Download,
            CloseButtonText = LocalizationDialog.Base_Close,
        };

        SetupDialog(dialog);

        return await dialog.ShowAsync();
    }

    private void SetupDialog(ContentDialog dialog)
    {
        // TODO: 设置主题, 临时解决对话框主题问题 https://github.com/microsoft/microsoft-ui-xaml/issues/2331
        dialog.RequestedTheme = _appearanceSettingsService.Theme;
        dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
        Interaction.SetBehaviors(dialog, new() { new ContentDialogBehavior() });
    }
}
