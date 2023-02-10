using DotVast.HashTool.WinUI.Activation;
using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Views;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;

namespace DotVast.HashTool.WinUI.Services;

public sealed class ActivationService : IActivationService
{
    private readonly ILogger<ActivationService> _logger;
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IAppearanceSettingsService _appearanceSettingsService;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly ICheckUpdateService _checkUpdateService;
    private readonly IHashTaskService _hashTaskService;
    private UIElement? _shell = null;

    public ActivationService(
        ILogger<ActivationService> logger,
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IAppearanceSettingsService appearanceSettingsService,
        IPreferencesSettingsService preferencesSettingsService,
        ICheckUpdateService checkUpdateService,
        IHashTaskService hashTaskService)
    {
        _logger = logger;
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _appearanceSettingsService = appearanceSettingsService;
        _preferencesSettingsService = preferencesSettingsService;
        _checkUpdateService = checkUpdateService;
        _hashTaskService = hashTaskService;
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
        var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        _logger.LogInformation("激活类型: {Kind}", activatedEventArgs.Kind);

        // activationHandler 处理 Windows.ApplicationModel.Activation.AppActivationArguments
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activatedEventArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activatedEventArgs);
        }

        // defaultHandler 处理 Microsoft.UI.Xaml.LaunchActivatedEventArgs
        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _appearanceSettingsService.InitializeAsync().ConfigureAwait(false);
        await _preferencesSettingsService.InitializeAsync().ConfigureAwait(false);
    }

    private async Task StartupAsync()
    {
        await _appearanceSettingsService.StartupAsync();
        await _preferencesSettingsService.StartupAsync();
        await _checkUpdateService.StartupAsync();
        await _hashTaskService.StartupAsync();
    }
}
