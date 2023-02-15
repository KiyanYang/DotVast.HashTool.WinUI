using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Contracts.Services;
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(EncodingJsonConverter))]
    public Encoding? Encoding { get; set; }

    public IList<Hash> SelectedHashs { get; init; } = Array.Empty<Hash>();

    /// <summary>
    /// 结果.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;

    /// <summary>
    /// 进度当前值.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    private double _progressVal;

    /// <summary>
    /// 进度最大值(计算完毕后等于 Results.Count).
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    private double _progressMax;

    #endregion Properties

    #region Manager

    private readonly ILogger<HashTask> _logger = App.GetLogger<HashTask>();
    private readonly IComputeHashService _computeHashService = App.GetService<IComputeHashService>();
    private readonly IDialogService _dialogService = App.GetService<IDialogService>();

    private CancellationTokenSource? _cts;
    private readonly ManualResetEventSlim _mres = new(true);

    public async Task StartAsync()
    {
        _cts ??= new();

        try
        {
            switch (Mode)
            {
                case var m when m == HashTaskMode.Text:
                    await _computeHashService.HashTextAsync(this, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.File:
                    await _computeHashService.HashFileAsync(this, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.Folder:
                    await _computeHashService.HashFolderAsync(this, _mres, _cts.Token);
                    break;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            await _dialogService.ShowInfoDialogAsync(
                LocalizationDialog.HashTaskAborted_Title,
                LocalizationDialog.HashTaskAborted_UnauthorizedAccess,
                LocalizationDialog.Base_OK);
            _logger.LogWarning("计算哈希时出现“未授权访问”异常, 模式: {Mode}, 内容: {Content}\n{Exception}", Mode, Content, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("计算哈希时出现未预料的异常, 哈希任务: {HashTask:j}\n{Exception}", this, ex);
        }
        finally
        {
            _cts.Dispose();
            _cts = null;
        }
    }

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

    public void Cancel()
    {
        _cts?.Cancel();
        _mres.Set();
    }

    #endregion Manager

    public override string ToString() =>
        $"{{ Content: `{Content}`, Mode: `{Mode}`, SelectedHashes: `{JsonSerializer.Serialize(SelectedHashs)}` }}";

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

        Cancel();

        if (disposing)
        {
            _mres.Dispose();
        }

        _disposed = true;
    }

    #endregion Finalizer, IDisposable
}

sealed file class EncodingJsonConverter : JsonConverter<Encoding?>
{
    public override Encoding? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var name = reader.GetString();
        return Encoding.GetEncodings()
                       .FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.OrdinalIgnoreCase))?
                       .GetEncoding();
    }

    public override void Write(Utf8JsonWriter writer, Encoding? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.WebName);
    }
}
