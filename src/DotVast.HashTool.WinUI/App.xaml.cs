using System.Diagnostics;

using DotVast.HashTool.WinUI.Activation;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Controls;
using DotVast.HashTool.WinUI.Helpers;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Services;
using DotVast.HashTool.WinUI.Services.Settings;
using DotVast.HashTool.WinUI.ViewModels;
using DotVast.HashTool.WinUI.ViewModels.Controls;
using DotVast.HashTool.WinUI.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
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
    public IHost Host { get; }

    public static T GetService<T>()
        where T : class
    {
        if (Current.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static ILogger<T> GetLogger<T>() where T : class => GetService<ILogger<T>>();

    public static new App Current => (App)Application.Current;

    public static MainWindow MainWindow { get; } = new();

    private readonly ILogger<App> _logger;
    private readonly long _createdTimestamp = Stopwatch.GetTimestamp();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            ConfigureHttpClient(services);
            ConfigureSingletonAsDefault(services);

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, CommandLineActivationHandler>();

            // Services
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IDispatchingService, DispatchingService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IAppearanceSettingsService, AppearanceSettingsService>();
            services.AddSingleton<IPreferencesSettingsService, PreferencesSettingsService>();

            services.AddSingleton<ICheckUpdateService, CheckUpdateService>();
            services.AddTransient<IComputeHashService, ComputeHashService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IHashTaskService, HashTaskService>();

            services.AddSingleton<IExportResolver, JsonExportResolver>();
            services.AddSingleton<IExportResolver, TextExportResolver>();

            // Core Services

            // Views and ViewModels
            services.AddTransient<HashSettingsViewModel>();
            services.AddTransient<HashSettingsPage>();
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

            services.AddTransient<HashTaskGrid>();
            services.AddTransient<HashTaskGridViewModel>();

            // Configuration
            services.Configure<DataOptions>(context.Configuration.GetSection(nameof(DataOptions)));
        }).
        UseSerilog((context, services, loggerConfiguration) =>
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.LogsOptions.FilePath);
            loggerConfiguration.WriteTo.File(path, shared: true, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3);
#if DEBUG
            loggerConfiguration.MinimumLevel.Debug();
#endif
        }).
        Build();

        UnhandledException += App_UnhandledException;

        _logger = GetLogger<App>();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        _logger.AppUnhandledException(e.Exception);
        e.Handled = true;

        try
        {
            var notificationService = GetService<INotificationService>();
            notificationService.Show(new()
            {
                Title = LocalizationDialog.UnhandledException_Title_UnhandledException,
                Message = LocalizationDialog.UnhandledException_Message_UnhandledExceptionOccurred,
                Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error,
                Duration = TimeSpan.FromSeconds(3),
            });
        }
        catch
        {
        }
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
#if DEBUG
        if (Debugger.IsAttached)
        {
            //DebugSettings.IsTextPerformanceVisualizationEnabled = true;
        }
#endif

        // 该表达式要在 IActivationService.ActivateAsync() 之前.
        Current.Resources[nameof(AppearanceSettingsService)] = GetService<IAppearanceSettingsService>();

        GetService<IDispatchingService>().Initialize(DispatcherQueue.GetForCurrentThread());
        await GetService<IActivationService>().ActivateAsync(args);

        _logger.AppLaunchedElapsedTime(Stopwatch.GetElapsedTime(_createdTimestamp));
    }

    private static void ConfigureHttpClient(IServiceCollection services)
    {
        services.AddHttpClient(Constants.HttpClient.GitHubRestApi, client =>
        {
            client.BaseAddress = new(Constants.GitHubRestApi.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);

            // https://docs.github.com/en/rest/overview/media-types
            // https://docs.github.com/en/rest/overview/resources-in-the-rest-api#user-agent-required
            client.DefaultRequestHeaders.Accept.Add(new("application/vnd.github+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new("DotVast.HashTool.WinUI", RuntimeHelper.AppVersion.ToString()));
        });
    }

    /// <summary>
    /// 配置单例并作为类的默认值使用.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    private static void ConfigureSingletonAsDefault(IServiceCollection services)
    {
        //services.AddSingleton<EncodingInfoJsonConverter>();
    }
}
