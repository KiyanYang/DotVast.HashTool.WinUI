using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IDialogService
{
    Task<ContentDialogResult> ShowInfoDialogAsync(string title, string content, string closeButtonText);
}
