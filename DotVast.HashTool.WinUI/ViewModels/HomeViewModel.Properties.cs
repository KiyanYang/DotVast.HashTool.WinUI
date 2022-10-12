using System.Text;

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
    private HashTaskMode _inputtingMode = HashTaskMode.File;

    /// <summary>
    /// 当前界面输入的哈希任务内容.
    /// </summary>
    [ObservableProperty]
    private string _inputtingContent = string.Empty;

    /// <summary>
    /// 当前界面输入的哈希任务文本编码.
    /// </summary>
    [ObservableProperty]
    private TextEncoding _inputtingTextEncoding = new("UTF-8", Encoding.UTF8);

    #endregion Inputting

    /// <summary>
    /// 哈希任务模式. 文件, 文件夹, 文本.
    /// </summary>
    public HashTaskMode[] HashTaskModes { get; } = new[] { HashTaskMode.File, HashTaskMode.Folder, HashTaskMode.Text };

    private IList<TextEncoding>? _textEncodings;

    /// <summary>
    /// 文本编码.
    /// </summary>
    public IList<TextEncoding> TextEncodings
    {
        get
        {
            if (_textEncodings == null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _textEncodings = Encoding.GetEncodings()
                    .Select(e => new TextEncoding(e.Name.ToUpper(), e.GetEncoding()))
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            return _textEncodings;
        }
    }

    /// <summary>
    /// Hash 选项.
    /// </summary>
    public IEnumerable<HashOption> HashOptions => _preferencesSettingsService.HashOptions.Where(i => i.IsEnabled);

    /// <summary>
    /// 当前流计算的进度.
    /// </summary>
    public ProgressBarModel AtomProgressBar { get; } = new() { Min = 0, Max = 1, Val = 0 };

    /// <summary>
    /// 当前任务的进度.
    /// </summary>
    public ProgressBarModel TaskProgressBar { get; } = new() { Min = 0, Max = 1, Val = 0 };

    /// <summary>
    /// “开始”按钮.
    /// </summary>
    public ButtonModel StartButton { get; } = new() { Content = Localization.Home_Button_Start };

    /// <summary>
    /// “暂停”和“继续”按钮.
    /// </summary>
    public ButtonModel ResetButton { get; } = new() { Content = Localization.Home_Button_Pause };

    /// <summary>
    /// “取消”按钮.
    /// </summary>
    public ButtonModel CancelButton { get; } = new() { Content = Localization.Home_Button_Cancel };

    /// <summary>
    /// 当前界面计算的哈希任务.
    /// </summary>
    [ObservableProperty]
    private HashTask? _currentHashTask;

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
