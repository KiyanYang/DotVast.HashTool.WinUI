// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ResultsViewModel(ILogger<ResultsViewModel> logger,
    IDialogService dialogService) : ObservableObject, IViewModel, INavigationAware
{
    private readonly ILogger<ResultsViewModel> _logger = logger;
    private readonly IDialogService _dialogService = dialogService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    public partial HashTask? HashTask { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    public partial string HashResultsFilter { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    public partial bool HashResultsFilterIsEnabled { get; set; } = false;

    private IList<HashResult>? _hashResultsFiltered;

    public IList<HashResult>? HashResultsFiltered
    {
        get
        {
            var filter = HashResultsFilter;
            if (string.IsNullOrEmpty(filter))
            {
                _hashResultsFiltered = HashTask?.Results;
            }
            else if (HashResultsFilterIsEnabled)
            {
                _hashResultsFiltered = HashTask?.Results?.Where(h =>
                    IgnoreCaseContains(h.Path, filter) || (h.Data?.Any(d => IgnoreCaseContains(d.Value, filter)) ?? false)
                ).ToArray();
            }
            return _hashResultsFiltered;
        }
        private set => SetProperty(ref _hashResultsFiltered, value);
    }

    [RelayCommand(CanExecute = nameof(CanConfigure))]
    private async Task ConfigureAsync()
    {
        if (HashTask?.HashOptions is { } hashOptions)
        {
            await _dialogService.ShowHashResultConfigDialogAsync(hashOptions);
        }
    }

    private bool CanConfigure() =>
        HashTask?.HashOptions is not null;

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        if (parameter is HashTask hashTask)
        {
            HashTask = hashTask;
        }
        else if (parameter != null)
        {
            _logger.LogError("导航事件参数传递的参数类型不是 HashTask. 参数: {Parameter}", parameter);
            throw new ArgumentException("导航事件参数传递的参数类型不是 HashTask");
        }
    }

    void INavigationAware.OnNavigatedFrom() { }

    #endregion INavigationAware

    partial void OnHashTaskChanging(HashTask? oldValue, HashTask? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.PropertyChanged -= HashTask_PropertyChanged;
        }

        if (newValue is not null)
        {
            newValue.PropertyChanged += HashTask_PropertyChanged;
            HashResultsFilter = string.Empty;
            HashResultsFilterIsEnabled = newValue.State != HashTaskState.Waiting && newValue.State != HashTaskState.Working;
        }
    }

    private void HashTask_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not HashTask hashTask)
        {
            return;
        }

        switch (e.PropertyName)
        {
            case nameof(HashTask.State):
                // 当任务处于进行状态时, 禁用搜索
                HashResultsFilterIsEnabled = hashTask.State != HashTaskState.Waiting && hashTask.State != HashTaskState.Working;
                break;

            case nameof(HashTask.Results):
                HashResultsFiltered = hashTask.Results;
                break;

            default:
                break;
        }
    }

    private static bool IgnoreCaseContains(string str1, string str2) =>
        str1.Contains(str2, StringComparison.OrdinalIgnoreCase);
}
