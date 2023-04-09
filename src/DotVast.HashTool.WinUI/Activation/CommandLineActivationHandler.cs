using System.Collections.Specialized;

using DotVast.HashTool.WinUI.Enums;

using Microsoft.Extensions.Logging;
using Microsoft.Windows.AppLifecycle;

using Windows.ApplicationModel.Activation;

namespace DotVast.HashTool.WinUI.Activation;

public sealed partial class CommandLineActivationHandler : ActivationHandler<AppActivationArguments>
{
    private record struct PathWithMode(string Path, HashTaskMode Mode);
    private readonly ILogger<CommandLineActivationHandler> _logger;
    private readonly INavigationService _navigationService;
    private readonly IHashService _hashService;
    private readonly IHashTaskService _hashTaskService;

    public CommandLineActivationHandler(ILogger<CommandLineActivationHandler> logger,
        INavigationService navigationService,
        IHashService hashService,
        IHashTaskService hashTaskService)
    {
        _logger = logger;
        _navigationService = navigationService;
        _hashService = hashService;
        _hashTaskService = hashTaskService;
    }

    protected override bool CanHandleInternal(AppActivationArguments args)
    {
        return args.Kind == ExtendedActivationKind.Launch
            && args.Data is ILaunchActivatedEventArgs launchArgs
            && launchArgs.Arguments.Length > 0;
    }

    protected override async Task HandleInternalAsync(AppActivationArguments args)
    {
        var launchArgs = (ILaunchActivatedEventArgs)args.Data;
        _logger.LaunchActivated(launchArgs.GetType(), launchArgs.Kind, launchArgs.Arguments);

        try
        {
            var cmdLineArgs = Environment.GetCommandLineArgs(); // this way instead of launchArgs.Arguments
            var parsedArgs = Parse(cmdLineArgs);
            var hashNames = parsedArgs.GetValues(Constants.CommandLineArgs.Hash);
            if (hashNames is null)
                return;

            var hashKinds = _hashService.GetHashes(hashNames);
            if (hashKinds.Length <= 0)
                return;

            var paths = parsedArgs.GetValues(Constants.CommandLineArgs.Path);
            if (paths is null)
                return;

            var pathWithModes = paths.Select(CreatePathWithMode).OfType<PathWithMode>().ToArray();
            if (pathWithModes.Length <= 0)
                return;

            foreach (var (path, mode) in pathWithModes)
            {
                _hashTaskService.HashTasks.Add(new()
                {
                    Mode = mode,
                    Content = path.Trim(),
                    SelectedHashKinds = hashKinds,
                    State = HashTaskState.Waiting,
                });
            }

            _navigationService.NavigateTo(PageKey.TasksPage);
        }
        catch (Exception e)
        {
            _logger.LaunchActivatedException(e);
        }

        await Task.CompletedTask;

        static PathWithMode? CreatePathWithMode(string path)
        {
            if (File.Exists(path))
            {
                return new(path, HashTaskMode.Files);
            }
            if (Directory.Exists(path))
            {
                return new(path, HashTaskMode.Folder);
            }
            return null;
        }
    }

    private static NameValueCollection Parse(string[] cmdLineArgs)
    {
        var ret = new NameValueCollection();
        string? key = null;
        foreach (var arg in cmdLineArgs)
        {
            if (arg.StartsWith("--"))
            {
                key = arg;
            }
            else if (key is not null)
            {
                ret.Add(key, arg);
            }
        }
        return ret;
    }
}
