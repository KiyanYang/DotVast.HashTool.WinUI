using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HomeViewModel
{
    #region Inputting

    /// <summary>
    /// 当前界面输入的哈希任务模式.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
    private HashTaskMode _inputtingMode = HashTaskMode.File;

    /// <summary>
    /// 当前界面输入的哈希任务内容.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
    private string _inputtingContent = string.Empty;

    /// <summary>
    /// 当前界面输入的哈希任务文本编码.
    /// </summary>
    [ObservableProperty]
    private TextEncoding _inputtingTextEncoding = TextEncoding.UTF8;

    #endregion Inputting

    /// <summary>
    /// 哈希任务模式. 文件, 文件夹, 文本.
    /// </summary>
    public HashTaskMode[] HashTaskModes { get; } = GenericEnum.GetFieldValues<HashTaskMode>();

    /// <summary>
    /// 文本编码.
    /// </summary>
    public IList<TextEncoding> TextEncodings => TextEncoding.All;

    /// <summary>
    /// Hash 选项.
    /// </summary>
    public IEnumerable<HashOption> HashOptions => _preferencesSettingsService.HashOptions.Where(i => i.IsEnabled);

    /// <summary>
    /// 是否处于任务延迟创建状态. 若处于延迟状态, 则当前无法创建任务.
    /// </summary>
    [ObservableProperty]
    private bool _isDelayCreateTask = false;

    /// <summary>
    /// 最近一次任务.
    /// </summary>
    public HashTask? LastHashTask => _hashTaskService.HashTasks.LastOrDefault();

    #region StartingWhenCreateHashTask

    [ObservableProperty]
    private bool _startingWhenCreateHashTask;

    partial void OnStartingWhenCreateHashTaskChanged(bool value) =>
        _preferencesSettingsService.StartingWhenCreateHashTask = value;

    #endregion StartingWhenCreateHashTask
}
