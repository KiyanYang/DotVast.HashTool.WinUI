using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashOption : ObservableRecipient
{
    public Hash Hash { get; }

    /// <summary>
    /// 是否选择该项.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isChecked;

    /// <summary>
    /// 是否启用该项.
    /// </summary>
    /// <remarks>
    /// 即控制该项是否在 HomePage 和 FileExplorerContextMenus 的显示.
    /// </remarks>
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isEnabled;

    public HashOption(Hash hash, bool isChecked = false, bool isEnabled = true)
    {
        Hash = hash;
        _isChecked = isChecked;
        _isEnabled = isEnabled;
    }
}
