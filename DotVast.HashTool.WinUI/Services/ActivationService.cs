using DotVast.HashTool.WinUI.Activation;
using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Services;

public sealed class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IAlwaysOnTopService _alwaysOnTopService;
    private readonly IHashOptionsService _hashOptionsService;
    private readonly ILanguageSelectorService _languageSelectorService;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell = null;

    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IAlwaysOnTopService alwaysOnTopService,
        IHashOptionsService hashOptionsService,
        ILanguageSelectorService languageSelectorService,
        IThemeSelectorService themeSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _alwaysOnTopService = alwaysOnTopService;
        _hashOptionsService = hashOptionsService;
        _languageSelectorService = languageSelectorService;
        _themeSelectorService = themeSelectorService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await _languageSelectorService.InitializeAsync().ConfigureAwait(false);
        await _hashOptionsService.InitializeAsync().ConfigureAwait(false);
        await _alwaysOnTopService.InitializeAsync().ConfigureAwait(false);
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await _alwaysOnTopService.SetRequestedIsAlwaysOnTopAsync();
        await Task.CompletedTask;
    }
}
