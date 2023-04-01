using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashOption : ObservableObject
{
    public Hash Hash { get; }

    /// <summary>
    /// 是否选择该项.
    /// </summary>
    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashOptionIsCheckedChangedMessage(this, value));

    /// <summary>
    /// 是否启用该项.
    /// </summary>
    /// <remarks>
    /// 即控制该项是否在 HomePage 和 FileExplorerContextMenus 的显示.
    /// </remarks>
    [ObservableProperty]
    private bool _isEnabled;

    partial void OnIsEnabledChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashOptionIsEnabledChangedMessage(this, value));

    public HashOption(Hash hash, bool isChecked = false, bool isEnabled = true)
    {
        Hash = hash;
        _isChecked = isChecked;
        _isEnabled = isEnabled;
    }
}
