using System.Collections.Specialized;
using System.Runtime.InteropServices;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Windows.AppLifecycle;

using Windows.ApplicationModel.Activation;

namespace DotVast.HashTool.WinUI.Activation;

public sealed partial class CommandLineActivationHandler : ActivationHandler<AppActivationArguments>
{
    public record struct PathWithMode(string Path, HashTaskMode Mode);
    private readonly ILogger<CommandLineActivationHandler> _logger;
    private readonly INavigationService _navigationService;
    private readonly IHashTaskService _hashTaskService;

    public CommandLineActivationHandler(ILogger<CommandLineActivationHandler> logger,
        INavigationService navigationService,
        IHashTaskService hashTaskService)
    {
        _logger = logger;
        _navigationService = navigationService;
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
            var parsedArgs = Parse(launchArgs.Arguments);
            var hashNames = parsedArgs.GetValues(Constants.CommandLineArgs.Hash);
            if (hashNames is null)
                return;

            var hashes = GetHashesFromNames(hashNames);
            if (hashes.Length <= 0)
                return;

            var paths = parsedArgs.GetValues(Constants.CommandLineArgs.Path);
            if (paths is null)
                return;

            var pathWithModes = paths.Select(CreatePathWithMode).OfType<PathWithMode>().ToArray();
            if (pathWithModes.Length <= 0)
                return;

            foreach (var (path, mode) in pathWithModes)
            {
                _hashTaskService.HashTasks.Add(CreateHashTask(hashes, path, mode));
            }

            _navigationService.NavigateTo(Constants.PageKeys.TaskPage);
        }
        catch (Exception e)
        {
            _logger.LaunchActivatedException(e);
        }

        await Task.CompletedTask;
    }

    [LibraryImport("shell32.dll", SetLastError = true)]
    private static partial IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

    private static string[] CommandLineToArgs(string commandLine)
    {
        var argv = CommandLineToArgvW(commandLine, out var argc);
        if (argv == IntPtr.Zero)
            throw new System.ComponentModel.Win32Exception();
        try
        {
            var args = new string[argc];
            for (var i = 0; i < args.Length; i++)
            {
                var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                args[i] = Marshal.PtrToStringUni(p)!;
            }

            return args;
        }
        finally
        {
            Marshal.FreeHGlobal(argv);
        }
    }

    private static NameValueCollection Parse(string argString)
    {
        var args = CommandLineToArgs(argString);
        var ret = new NameValueCollection();
        string? key = null;
        foreach (var arg in args)
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

    private static Hash[] GetHashesFromNames(string[] names)
    {
        return Hash.All.IntersectBy(names, h => h.Name, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// 从路径获取 <see cref="HashTaskMode"/> 值.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>
    /// 文件则返回 <see cref="HashTaskMode.File"/>;
    /// 文件夹则返回 <see cref="HashTaskMode.Folder"/>;
    /// 无效路径则返回 <see langword="null"/>.
    /// </returns>
    private static PathWithMode? CreatePathWithMode(string path)
    {
        if (File.Exists(path))
        {
            return new(path, HashTaskMode.File);
        }
        if (Directory.Exists(path))
        {
            return new(path, HashTaskMode.Folder);
        }
        return null;
    }

    private static HashTask CreateHashTask(Hash[] hashes, string path, HashTaskMode mode)
    {
        return new()
        {
            DateTime = DateTime.Now,
            Mode = mode,
            Content = path.Trim(),
            SelectedHashs = hashes,
            State = HashTaskState.Waiting,
        };
    }
}
