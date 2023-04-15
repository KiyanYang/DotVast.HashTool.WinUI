using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ResultsViewModel : ObservableObject, IViewModel, INavigationAware
{
    private readonly ILogger<ResultsViewModel> _logger;

    public ResultsViewModel(ILogger<ResultsViewModel> logger)
    {
        _logger = logger;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private HashTask? _hashTask;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private string _hashResultsFilter = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private bool _hashResultsFilterIsEnabled = false;

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

    partial void OnHashTaskChanged(HashTask? value)
    {
        if (HashTask != null)
        {
            HashTask.PropertyChanged -= HashTask_PropertyChanged;
        }

        if (value != null)
        {
            value.PropertyChanged += HashTask_PropertyChanged;
            HashResultsFilter = string.Empty;
            HashResultsFilterIsEnabled = value.State != HashTaskState.Waiting && value.State != HashTaskState.Working;
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
