using DotVast.HashTool.WinUI.Contracts.ViewModels;

namespace DotVast.HashTool.WinUI.ViewModels;

partial class HomeViewModel : INavigationAware
{
    public void OnNavigatedFrom() { }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is string[]) // CommandLineArgs
        {
            // TODO: process CommandLineArgs
        }
    }
}
