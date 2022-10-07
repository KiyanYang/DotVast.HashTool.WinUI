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
    /// 结果.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;

    public IList<Hash> SelectedHashs { get; set; } = Array.Empty<Hash>();
}
