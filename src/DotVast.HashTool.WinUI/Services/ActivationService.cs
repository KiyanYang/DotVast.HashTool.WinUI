// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Activation;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Views;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

using WinUIEx;

namespace DotVast.HashTool.WinUI.Services;

public sealed class ActivationService(
    ILogger<ActivationService> logger,
    ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
    IEnumerable<IActivationHandler> activationHandlers,
    IAppearanceSettingsService appearanceSettingsService,
    IPreferencesSettingsService preferencesSettingsService,
    ICheckUpdateService checkUpdateService,
    IHashTaskService hashTaskService) : IActivationService
{
    private readonly ILogger<ActivationService> _logger = logger;
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler = defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers = activationHandlers;
    private readonly IAppearanceSettingsService _appearanceSettingsService = appearanceSettingsService;
    private readonly IPreferencesSettingsService _preferencesSettingsService = preferencesSettingsService;
    private readonly ICheckUpdateService _checkUpdateService = checkUpdateService;
    private readonly IHashTaskService _hashTaskService = hashTaskService;

    public async Task ActivateAsync(object activationArgs)
    {
        // Set the MainWindow Content.
        App.MainWindow.Content ??= App.GetService<ShellPage>();

        // Execute tasks before activation.
        await InitializeAsync();

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        App.MainWindow.CenterOnScreen();

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

        // activationHandler 处理 Microsoft.Windows.AppLifecycle.AppActivationArguments
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activatedEventArgs));

        if (activationHandler != null)
        {
            _logger.HandleLaunchActivation(activatedEventArgs.Kind);
            await activationHandler.HandleAsync(activatedEventArgs);
        }

        // defaultHandler 处理 Microsoft.UI.Xaml.LaunchActivatedEventArgs
        if (_defaultHandler.CanHandle(activationArgs))
        {
            _logger.HandleDefaultLaunchActivation(((LaunchActivatedEventArgs)activationArgs).Arguments);
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
