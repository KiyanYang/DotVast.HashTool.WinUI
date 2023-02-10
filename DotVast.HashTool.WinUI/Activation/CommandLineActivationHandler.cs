using System.Collections.Specialized;
using System.Runtime.InteropServices;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.Extensions.Logging;
using Microsoft.Windows.AppLifecycle;

using Windows.ApplicationModel.Activation;

namespace DotVast.HashTool.WinUI.Activation;

public sealed partial class CommandLineActivationHandler : ActivationHandler<AppActivationArguments>
{
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
            && args.Data is LaunchActivatedEventArgs data
            && data.Arguments.Length > 0;
    }

    protected override async Task HandleInternalAsync(AppActivationArguments args)
    {
        var data = (LaunchActivatedEventArgs)args.Data;
        _logger.LogInformation("激活参数: {@Args}", data);

        var parsedArgs = Parse(data.Arguments);
        var hashNames = parsedArgs.GetValues(Constants.CommandLineArgs.Hash)!;
        var hashes = GetHashesFromNames(hashNames);
        var paths = parsedArgs.GetValues(Constants.CommandLineArgs.Path)!;
        var pathWithModes = paths.Select(p => (p, GetHashTaskModeFromPath(p)));

        foreach (var (path, mode) in pathWithModes)
        {
            _hashTaskService.HashTasks.Add(CreateHashTask(hashes, path, mode));
        }

        _navigationService.NavigateTo(typeof(TasksViewModel).FullName!, args);

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
        var allHashes = Hash.All;
        return names.Select(x => allHashes.FirstOrDefault(i => i.Name.Equals(x, StringComparison.OrdinalIgnoreCase)))
            .Where(x => x is not null)
            .ToArray()!;
    }

    private static HashTaskMode GetHashTaskModeFromPath(string path)
    {
        var attributes = File.GetAttributes(path);

        return attributes.HasFlag(FileAttributes.Directory) ? HashTaskMode.Folder : HashTaskMode.File;
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
