using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ResultsViewModel : ObservableRecipient
{
    public ResultsViewModel()
    {
    }

    [ObservableProperty]
    private HashTask? _hashTask;

    [ObservableProperty]
    private Visibility _encodingItemVisibility;

    partial void OnHashTaskChanged(HashTask? value)
    {
        EncodingItemVisibility = value?.Mode == HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;
    }
}
