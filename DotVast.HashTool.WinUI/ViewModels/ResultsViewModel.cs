using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ResultsViewModel : ObservableRecipient
{
    public ResultsViewModel()
    {
    }

    [ObservableProperty]
    private HashTask? _hashTask;
}
