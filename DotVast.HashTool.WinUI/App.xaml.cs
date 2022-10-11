using System.Diagnostics;

using DotVast.HashTool.WinUI.Activation;
using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Core.Contracts.Services;
using DotVast.HashTool.WinUI.Core.Services;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Services;
using DotVast.HashTool.WinUI.Services.Settings;
using DotVast.HashTool.WinUI.ViewModels;
using DotVast.HashTool.WinUI.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

using Serilog;

namespace DotVast.HashTool.WinUI;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public sealed partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static ILogger<T> GetLogger<T>() where T : class => GetService<ILogger<T>>();

    public static IOptions<T> GetOptions<T>() where T : class => GetService<IOptions<T>>();

    public static WindowEx MainWindow { get; } = new MainWindow();

    private readonly ILogger<App> _logger;
    private Stopwatch? _stopwatch;

    public App()
    {
        _stopwatch = Stopwatch.StartNew();

        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IAppearanceSettingsService, AppearanceSettingsService>();
            services.AddSingleton<IPreferencesSettingsService, PreferencesSettingsService>();
            services.AddSingleton<IComputeHashService, ComputeHashService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IHashTaskService, HashTaskService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<HashOptionSettingsViewModel>();
            services.AddTransient<HashOptionSettingsPage>();
            services.AddTransient<LicensesViewModel>();
            services.AddTransient<LicensesPage>();
            services.AddTransient<ResultsViewModel>();
            services.AddTransient<ResultsPage>();
            services.AddTransient<TasksViewModel>();
            services.AddTransient<TasksPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.Configure<LogsOptions>(context.Configuration.GetSection(nameof(LogsOptions)));
        }).
        UseSerilog((context, services, loggerConfiguration) =>
        {
            var filePath = services.GetService<IOptions<LogsOptions>>()!.Value.FullPath;
            loggerConfiguration
                .WriteTo.File(filePath, shared: true, rollingInterval: RollingInterval.Day);
        }).
        Build();

        UnhandledException += App_UnhandledException;

        _logger = GetLogger<App>();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        _logger.LogCritical("未处理的异常: {Message}\n{Exception}", e.Message, e.Exception);
        e.Handled = true;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
#if DEBUG
        if (Debugger.IsAttached)
        {
            //DebugSettings.IsTextPerformanceVisualizationEnabled = true;
        }
#endif
        base.OnLaunched(args);

        // TODO: Resources 里的 DataTemplate 不能绑定到 ViewModel, 因此使用静态资源访问.
        // 该表达式要在 IActivationService.ActivateAsync() 之前.
        App.Current.Resources[nameof(AppearanceSettingsService)] = App.GetService<IAppearanceSettingsService>();

        await App.GetService<IActivationService>().ActivateAsync(args);

        TitleBarContextMenuHelper.SetTitleBarContextMenuAllowDark();

        _stopwatch!.Stop();
        _logger.LogInformation("软件已启动, 用时: {LaunchedElapsed} ms.", _stopwatch!.ElapsedMilliseconds);
        _stopwatch = null;
    }
}
