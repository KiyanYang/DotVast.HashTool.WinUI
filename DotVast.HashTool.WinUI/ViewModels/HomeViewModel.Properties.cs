using System.Text;

using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Controls;

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
    private TextEncoding _inputtingTextEncoding = new(Encoding.UTF8.WebName.ToUpper(), new(() => Encoding.UTF8));

    #endregion Inputting

    /// <summary>
    /// 哈希任务模式. 文件, 文件夹, 文本.
    /// </summary>
    public HashTaskMode[] HashTaskModes { get; } = GenericEnum.GetFieldValues<HashTaskMode>();

    /// <summary>
    /// 文本编码.
    /// </summary>
    public IList<TextEncoding> TextEncodings
    {
        get
        {
            if (_textEncodings is null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _textEncodings = Encoding.GetEncodings()
                    .Select(e => new TextEncoding(e.Name.ToUpper(), new(() => e.GetEncoding())))
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            return _textEncodings;
        }
    }
    private IList<TextEncoding>? _textEncodings;

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

    #region Verifying Hash

    private string _verifyingHash1 = string.Empty;
    private string _verifyingHash2 = string.Empty;

    public string VerifyingHash1
    {
        get => _verifyingHash1;
        set
        {
            if (SetProperty(ref _verifyingHash1, value.Trim()))
            {
                OnPropertyChanged(nameof(VerifyingResult));
            }
        }
    }

    public string VerifyingHash2
    {
        get => _verifyingHash2;
        set
        {
            if (SetProperty(ref _verifyingHash2, value.Trim()))
            {
                OnPropertyChanged(nameof(VerifyingResult));
            }
        }
    }

    public string? VerifyingResult
    {
        get
        {
            if (VerifyingHash1 == string.Empty || VerifyingHash2 == string.Empty)
            {
                return null;
            }

            if (string.Equals(VerifyingHash1, VerifyingHash2, StringComparison.Ordinal))
            {
                return "=";
            }

            // Quick Xor, Base64, 必须完全相等
            if (!VerifyingHash1.EndsWith('=')
                && string.Equals(VerifyingHash1, VerifyingHash2, StringComparison.OrdinalIgnoreCase))
            {
                return "≈";
            }

            return "≠";
        }
    }

    #endregion Verifying Hash

    public TeachingTipModel TipMessage { get; } = new();
}
