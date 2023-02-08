using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

namespace DotVast.HashTool.WinUI.Activation;

public sealed class CommandLineActivationHandler : ActivationHandler<string[]>
{
    private readonly INavigationService _navigationService;
    private readonly IHashTaskService _hashTaskService;

    public CommandLineActivationHandler(INavigationService navigationService,
        IHashTaskService hashTaskService)
    {
        _navigationService = navigationService;
        _hashTaskService = hashTaskService;
    }

    protected override async Task HandleInternalAsync(string[] args)
    {
        var parsedArgs = Parse(args);
        var hashNames = parsedArgs[Constants.CommandLineArgs.Hash];
        var hashes = GetHashesFromNames(hashNames);
        var paths = parsedArgs[Constants.CommandLineArgs.Path];
        var pathWithModes = paths.Select(p => (p, GetHashTaskModeFromPath(p)));

        foreach (var (path, mode) in pathWithModes)
        {
            _hashTaskService.HashTasks.Add(CreateHashTask(hashes, path, mode));
        }

        _navigationService.NavigateTo(typeof(TasksViewModel).FullName!, args);

        await Task.CompletedTask;
    }

    private static Dictionary<string, string[]> Parse(string[] args)
    {
        var ret = new Dictionary<string, string[]>();
        string? key = null;
        List<string> values = new();
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                if (key is not null)
                {
                    ret.Add(key, values.ToArray());
                }
                key = arg;
                values.Clear();
            }
            else
            {
                values.Add(arg);
            }
        }
        if (key is not null)
        {
            ret.Add(key, values.ToArray());
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
            Content = path,
            SelectedHashs = hashes,
            State = HashTaskState.Waiting,
        };
    }
}
