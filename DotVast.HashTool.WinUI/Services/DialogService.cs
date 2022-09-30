using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.Contracts.Services;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DotVast.HashTool.WinUI.Services;

internal class DialogService : IDialogService
{
    public async Task<ContentDialogResult> ShowInfoDialogAsync(string title, string content, string closeButtonText)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            CloseButtonText = closeButtonText,
            XamlRoot = App.MainWindow.Content.XamlRoot,
        };

        Interaction.SetBehaviors(dialog, new() { new ContentDialogBehavior() });

        return await dialog.ShowAsync();
    }
}
