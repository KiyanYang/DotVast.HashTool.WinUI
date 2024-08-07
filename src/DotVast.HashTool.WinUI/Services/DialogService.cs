// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Views.Dialogs;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class DialogService(IAppearanceSettingsService appearanceSettingsService) : IDialogService
{
    private readonly IAppearanceSettingsService _appearanceSettingsService = appearanceSettingsService;

    public Task<ContentDialogResult> ShowDialogAsync(
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
            DefaultButton = ContentDialogButton.Primary
        };

        SetupDialog(dialog);

        return dialog.ShowAsync().AsTask();
    }

    public Task<ContentDialogResult> ShowGithubUpdateDialogAsync(GitHubRelease release)
    {
        var dialog = new GithubUpdateDialog
        {
            Release = release,
            PrimaryButtonText = LocalizationPopup.GitHubUpdate_Button_Download,
            CloseButtonText = LocalizationCommon.Close,
        };

        SetupDialog(dialog);

        return dialog.ShowAsync().AsTask();
    }

    public Task<ContentDialogResult> ShowHashResultConfigDialogAsync(IList<HashOption> hashOptions)
    {

        var dialog = new HashResultConfigDialog(hashOptions)
        {
            CloseButtonText = LocalizationCommon.Close,
        };

        SetupDialog(dialog);

        return dialog.ShowAsync().AsTask();
    }

    private void SetupDialog(ContentDialog dialog)
    {
        // TODO: 设置主题, 临时解决对话框主题问题 https://github.com/microsoft/microsoft-ui-xaml/issues/2331
        dialog.RequestedTheme = _appearanceSettingsService.Theme.ToElementTheme();
        dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
    }
}
