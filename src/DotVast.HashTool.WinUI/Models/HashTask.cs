using System.Collections.ObjectModel;
using System.Text;

using DotVast.HashTool.WinUI.Enums;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTask : ObservableObject, IDisposable
{
    #region Properties

    /// <summary>
    /// 任务创建时间.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// 用时.
    /// </summary>
    [ObservableProperty]
    private TimeSpan _elapsed;

    /// <summary>
    /// 任务状态.
    /// </summary>
    [ObservableProperty]
    private HashTaskState? _state;

    public HashTaskMode Mode { get; set; } = HashTaskMode.File;

    public string Content { get; set; } = string.Empty;

    public HashKind[] SelectedHashKinds { get; internal set; } = Array.Empty<HashKind>();

    /// <summary>
    /// 结果.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;

    /// <summary>
    /// 进度当前值.
    /// </summary>
    //[property: JsonIgnore]
    [ObservableProperty]
    private double _progressVal;

    /// <summary>
    /// 进度最大值(计算完毕后等于 Results.Count).
    /// </summary>
    //[property: JsonIgnore]
    [ObservableProperty]
    private double _progressMax;

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
        sb.Append($"{nameof(Content)} = {Content}");
        sb.Append($", {nameof(Mode)} = {Mode}");
        sb.Append($", {nameof(SelectedHashKinds)} = [ ");
        sb.Append(string.Join(", ", SelectedHashKinds));
        sb.Append(" ]");
        sb.Append(" }");
        return sb.ToString();
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
