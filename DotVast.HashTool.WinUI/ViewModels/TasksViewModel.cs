using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class TasksViewModel : ObservableRecipient
{
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;

    public TasksViewModel(IHashTaskService hashTaskService, INavigationService navigationService)
    {
        _hashTaskService = hashTaskService;
        _navigationService = navigationService;
        HashTasks = _hashTaskService.HashTasks;
    }

    public IList<HashTask> HashTasks
    {
        get;
    }

    [RelayCommand]
    private void ShowResult(HashTask hashTask)
    {
        // TODO: 新增结果页面后, 取消注释.
        //_navigationService.NavigateTo(typeof(ResultsViewModel).FullName!, parameter: hashTask);
    }
}
