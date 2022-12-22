using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.ViewModels;

namespace DotVast.HashTool.WinUI.Activation;

public sealed class CommandLineActivationHandler : ActivationHandler<string[]>
{
    private readonly INavigationService _navigationService;

    public CommandLineActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override async Task HandleInternalAsync(string[] args)
    {
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!, args);

        await Task.CompletedTask;
    }
}
