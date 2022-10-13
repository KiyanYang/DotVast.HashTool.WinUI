using System.Collections.ObjectModel;
using System.Text;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTask : ObservableObject
{
    /// <summary>
    /// ID.
    /// </summary>
    public int Id { get; set; }

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

    public Encoding? Encoding { get; set; }

    public IList<Hash> SelectedHashs { get; init; } = Array.Empty<Hash>();

    /// <summary>
    /// 结果.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;
}
