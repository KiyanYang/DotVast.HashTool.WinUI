using DotVast.HashTool.WinUI.Contracts.ViewModels;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class ResultsViewModel : ObservableRecipient, INavigationAware
{
    public ResultsViewModel()
    {
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private HashTask? _hashTask;

    [ObservableProperty]
    private Visibility _hashResultsVisibility;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private string _hashResultsFilterByContent = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HashResultsFiltered))]
    private bool _hashResultsFilterByContentIsEnabled = false;

    private IList<HashResult>? _hashResultsFiltered;

    public IList<HashResult>? HashResultsFiltered
    {
        get
        {
            if (_hashResultsFilterByContentIsEnabled)
            {
                return HashTask?.Results?.Where(i => i.Content.Contains(HashResultsFilterByContent, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return _hashResultsFiltered;
        }
        private set => SetProperty(ref _hashResultsFiltered, value);
    }

    #region INavigationAware

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is HashTask hashTask)
        {
            HashTask = hashTask;
        }
        else if (parameter != null)
        {
            throw new ArgumentException("导航事件参数传递的变量类型不是 HashTask");
        }
    }

    public void OnNavigatedFrom()
    {
    }

    #endregion INavigationAware

    partial void OnHashTaskChanging(HashTask? value)
    {
        if (_hashTask != null)
        {
            _hashTask.PropertyChanged -= HashTask_PropertyChanged;
        }

        if (value != null)
        {
            value.PropertyChanged += HashTask_PropertyChanged;
            HashResultsFilterByContent = string.Empty;
            HashResultsFilterByContentIsEnabled = value.State != HashTaskState.Waiting && value.State != HashTaskState.Working;
            HashResultsVisibility = value.Results != null ? Visibility.Visible : Visibility.Collapsed;
            HashResultsFiltered = value.Results;
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
                HashResultsFilterByContentIsEnabled = hashTask.State != HashTaskState.Waiting
                    && hashTask.State != HashTaskState.Working;
                break;

            case nameof(HashTask.Results):
                if (hashTask.Results != null)
                {
                    HashResultsVisibility = Visibility.Visible;
                    HashResultsFiltered = hashTask.Results;
                }
                else
                {
                    HashResultsVisibility = Visibility.Collapsed;
                }
                break;

            default:
                break;
        }
    }
}
