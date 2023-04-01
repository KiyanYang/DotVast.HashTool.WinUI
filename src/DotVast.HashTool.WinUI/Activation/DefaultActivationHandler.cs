using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Activation;

public sealed class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content is null;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(Constants.PageKeys.HomePage, args.Arguments);

        await Task.CompletedTask;
    }
}
