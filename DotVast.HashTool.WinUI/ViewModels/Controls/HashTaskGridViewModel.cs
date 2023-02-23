using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels.Controls;

public sealed partial class HashTaskGridViewModel : ObservableObject
{
    private readonly IHashTaskService _hashTaskService;

    public HashTaskGridViewModel(IHashTaskService hashTaskService)
    {
        _hashTaskService = hashTaskService;
    }

    [ObservableProperty]
    private HashTask? _hashTask;

    partial void OnHashTaskChanging(HashTask? value)
    {
        if (HashTask != null)
        {
            HashTask.PropertyChanged -= HashTask_PropertyChanged;
        }
        if (value != null)
        {
            value.PropertyChanged += HashTask_PropertyChanged;
        }
    }

    private void HashTask_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StartTaskCommand.NotifyCanExecuteChanged();
        ResetTaskCommand.NotifyCanExecuteChanged();
        CancelTaskCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanStartTask))]
    private async Task StartTaskAsync()
    {
        if (HashTask is null)
            return;

        await HashTask.StartAsync();
    }

    public bool CanStartTask() => HashTask?.State == HashTaskState.Waiting
        || HashTask?.State == HashTaskState.Completed
        || HashTask?.State == HashTaskState.Canceled
        || HashTask?.State == HashTaskState.Aborted;

    [RelayCommand(CanExecute = nameof(CanResetTask))]
    private void ResetTask()
    {
        HashTask?.Reset();
    }

    public bool CanResetTask() => HashTask?.State == HashTaskState.Paused
        || HashTask?.State == HashTaskState.Working;

    [RelayCommand(CanExecute = nameof(CanCancelTask))]
    private void CancelTask()
    {
        HashTask?.Cancel();
    }

    public bool CanCancelTask() => HashTask?.State == HashTaskState.Paused
        || HashTask?.State == HashTaskState.Working;

    [RelayCommand]
    public void DeleteTask()
    {
        if (HashTask is null)
        {
            return;
        }

        HashTask.Cancel();
        HashTask.Dispose();
        _hashTaskService.HashTasks.Remove(HashTask);
    }
}
