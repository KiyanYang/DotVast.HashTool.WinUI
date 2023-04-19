// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Navigation;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.ViewModels.Controls;

public sealed partial class HashTaskGridViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;

    public HashTaskGridViewModel(
        IDialogService dialogService,
        IHashTaskService hashTaskService,
        INavigationService navigationService)
    {
        _dialogService = dialogService;
        _hashTaskService = hashTaskService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(ResetTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteTaskCommand))]
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

    private bool CanStartTask() => HashTask?.State == HashTaskState.Waiting
        || HashTask?.State == HashTaskState.Completed
        || HashTask?.State == HashTaskState.Canceled
        || HashTask?.State == HashTaskState.Aborted;

    [RelayCommand(CanExecute = nameof(CanResetTask))]
    private void ResetTask()
    {
        HashTask?.Reset();
    }

    private bool CanResetTask() => HashTask?.State == HashTaskState.Paused
        || HashTask?.State == HashTaskState.Working;

    [RelayCommand(CanExecute = nameof(CanCancelTask))]
    private void CancelTask()
    {
        HashTask?.Cancel();
    }

    private bool CanCancelTask() => HashTask?.State == HashTaskState.Paused
        || HashTask?.State == HashTaskState.Working;

    [RelayCommand(CanExecute = nameof(CanDeleteTask))]
    private async Task DeleteTaskAsync()
    {
        var dialogResult = await _dialogService.ShowDialogAsync(
            LocalizationPopup.DeleteHashTask_Title_DeleteTask,
            LocalizationPopup.DeleteHashTask_Content_WantToDeleteThisTask,
            LocalizationCommon.No,
            primaryButtonText: LocalizationCommon.Yes);

        if (dialogResult != ContentDialogResult.Primary)
        {
            return;
        }

        HashTask!.Cancel();
        HashTask.Dispose();
        _hashTaskService.HashTasks.Remove(HashTask);
    }

    private bool CanDeleteTask() => HashTask is not null;

    [RelayCommand(CanExecute = nameof(CanEditTask))]
    private void EditTask()
    {
        _navigationService.NavigateTo(PageKey.HomePage, new HomeParameter(HomeParameterKind.EditHashTask, HashTask!));
    }

    private bool CanEditTask() => HashTask is not null;
}
