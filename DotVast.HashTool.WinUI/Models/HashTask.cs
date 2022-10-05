using System.Collections.ObjectModel;
using System.Text;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTask : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private DateTime _dateTime;

    [ObservableProperty]
    private TimeSpan _elapsed;

    [ObservableProperty]
    private HashTaskMode _mode = HashTaskMode.File;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private Encoding? _encoding;

    [ObservableProperty]
    private HashTaskState? _state;

    /// <summary>
    /// 单结果 (文件, 文本).
    /// </summary>
    [ObservableProperty]
    private HashResult? _result;

    /// <summary>
    /// 多结果.
    /// 在“文件, 文本”模式下，设置值为 new() { Result }.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;

    public IList<Hash> SelectedHashs { get; set; } = Array.Empty<Hash>();

    partial void OnResultChanged(HashResult? value)
    {
        if (Mode == HashTaskMode.Folder || value == null)
        {
            return;
        }

        if (Results != null)
        {
            Results.Clear();
            Results.Add(value);
        }
        else
        {
            Results = new() { value };
        }
    }
}
