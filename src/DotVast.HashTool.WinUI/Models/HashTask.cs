using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTask : ObservableObject, IDisposable
{
    #region Properties

    public HashTaskMode Mode { get; set; } = HashTaskMode.Files;

    public string Content { get; set; } = string.Empty;

    public HashKind[] SelectedHashKinds { get; internal set; } = Array.Empty<HashKind>();

    /// <summary>
    /// 结果.
    /// </summary>
    public ObservableCollection<HashResult>? Results
    {
        get => _results;
        set
        {
            if (!EqualityComparerEquals(_results, value))
            {
                OnPropertyChanging(s_resultsChangingEventArgs);
                _results = value;
                OnPropertyChanged(s_resultsChangedEventArgs);
            }
        }
    }
    private ObservableCollection<HashResult>? _results;
    private static readonly PropertyChangingEventArgs s_resultsChangingEventArgs = new(nameof(Results));
    private static readonly PropertyChangedEventArgs s_resultsChangedEventArgs = new(nameof(Results));

    /// <summary>
    /// 用时.
    /// </summary>
    public TimeSpan Elapsed
    {
        get => _elapsed;
        set
        {
            if (!EqualityComparerEquals(_elapsed, value))
            {
                OnPropertyChanging(s_elapsedChangingEventArgs);
                _elapsed = value;
                OnPropertyChanged(s_elapsedChangedEventArgs);
            }
        }
    }
    private TimeSpan _elapsed;
    private static readonly PropertyChangingEventArgs s_elapsedChangingEventArgs = new(nameof(Elapsed));
    private static readonly PropertyChangedEventArgs s_elapsedChangedEventArgs = new(nameof(Elapsed));

    /// <summary>
    /// 任务状态.
    /// </summary>
    public HashTaskState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                OnPropertyChanging(s_stateChangingEventArgs);
                _state = value;
                OnPropertyChanged(s_stateChangedEventArgs);
            }
        }
    }
    private HashTaskState _state;
    private static readonly PropertyChangingEventArgs s_stateChangingEventArgs = new(nameof(State));
    private static readonly PropertyChangedEventArgs s_stateChangedEventArgs = new(nameof(State));

    /// <summary>
    /// 进度当前值.
    /// </summary>
    [JsonIgnore]
    public double ProgressVal
    {
        get => _progressVal;
        set
        {
            if (_progressVal != value)
            {
                OnPropertyChanging(s_progressValChangingEventArgs);
                _progressVal = value;
                OnPropertyChanged(s_progressValChangedEventArgs);
            }
        }
    }
    private double _progressVal;
    private static readonly PropertyChangingEventArgs s_progressValChangingEventArgs = new(nameof(ProgressVal));
    private static readonly PropertyChangedEventArgs s_progressValChangedEventArgs = new(nameof(ProgressVal));

    /// <summary>
    /// 进度最大值(计算完毕后等于 Results.Count).
    /// </summary>
    [JsonIgnore]
    public double ProgressMax
    {
        get => _progressMax;
        set
        {
            if (_progressMax != value)
            {
                OnPropertyChanging(s_progressMaxChangingEventArgs);
                _progressMax = value;
                OnPropertyChanged(s_progressMaxChangedEventArgs);
            }
        }
    }
    private double _progressMax;
    private static readonly PropertyChangingEventArgs s_progressMaxChangingEventArgs = new(nameof(ProgressMax));
    private static readonly PropertyChangedEventArgs s_progressMaxChangedEventArgs = new(nameof(ProgressMax));

    #endregion Properties

    private readonly HashTaskManager _manager;

    public HashTask()
    {
        _manager = new(this);
    }

    public Task StartAsync() => _manager.StartAsync();
    public bool Reset() => _manager.Reset();
    public void Cancel() => _manager.Cancel();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"{nameof(HashTask)} {{ ");
        sb.Append($"{nameof(Content)} = '{Content}'");
        sb.Append($", {nameof(Mode)} = '{Mode}'");
        sb.Append($", {nameof(SelectedHashKinds)} = [ ");
        sb.AppendJoin(", ", SelectedHashKinds);
        sb.Append(" ]");
        sb.Append(" }");
        return sb.ToString();
    }

    private static bool EqualityComparerEquals<T>(T? x, T? y)
    {
        return EqualityComparer<T>.Default.Equals(x, y);
    }

    #region Finalizer, IDisposable

    private bool _disposed = false;

    ~HashTask() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _manager.Dispose();
        }

        _disposed = true;
    }

    #endregion Finalizer, IDisposable

    private sealed class HashTaskManager : IDisposable
    {
        private readonly ILogger<HashTaskManager> _logger = App.GetLogger<HashTaskManager>();
        private readonly IComputeHashService _computeHashService = App.GetService<IComputeHashService>();
        private readonly IDialogService _dialogService = App.GetService<IDialogService>();

        private readonly HashTask _hashTask;
        private readonly ManualResetEventSlim _mres = new(true);
        private CancellationTokenSource? _cts;

        public HashTaskManager(HashTask hashTask)
        {
            _hashTask = hashTask;
        }

        /// <summary>
        /// 开始计算.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _cts ??= new();

            try
            {
                await _computeHashService.ComputeHashAsync(_hashTask, _mres, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("计算哈希时操作被取消.");
            }
            catch (UnauthorizedAccessException ex)
            {
                await _dialogService.ShowDialogAsync(
                    LocalizationDialog.HashTaskAborted_Title_HashTaskAborted,
                    LocalizationDialog.HashTaskAborted_Content_UnauthorizedAccess,
                    LocalizationCommon.OK);
                _logger.LogWarning("计算哈希时出现“未授权访问”异常, 模式: {Mode}, 内容: {Content}\n{Exception}", _hashTask.Mode, _hashTask.Content, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("计算哈希时出现未预料的异常, 哈希任务: {HashTask}\n{Exception}", _hashTask, ex);
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        /// <summary>
        /// 暂停或恢复计算.
        /// </summary>
        /// <returns>当计算暂停后, 返回 <see langword="true"/>, 否则返回 <see langword="false"/>.</returns>
        public bool Reset()
        {
            if (_mres.IsSet)
            {
                _mres.Reset();
                return true;
            }
            else
            {
                _mres.Set();
                return false;
            }
        }

        /// <summary>
        /// 取消计算.
        /// </summary>
        public void Cancel()
        {
            _cts?.Cancel();
            _mres.Set();
        }

        #region Finalizer, IDisposable

        private bool _disposed = false;

        ~HashTaskManager() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            Cancel();

            if (disposing)
            {
                _mres.Dispose();
            }

            _disposed = true;
        }

        #endregion Finalizer, IDisposable
    }
}
