using System.Text.Json.Serialization;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashSetting : ObservableObject
{
    public HashKind Kind
    {
        get => _kind;
        init
        {
            _kind = value;
            Name = _kind.ToName();
        }
    }
    private HashKind _kind;

    /// <summary>
    /// 名称. 仅用作界面显示, 不要用于内部调用.
    /// </summary>
    [JsonIgnore]
    public string Name { get; private set; } = string.Empty;

    [ObservableProperty]
    private HashFormat _format;

    partial void OnFormatChanged(HashFormat value) =>
        WeakReferenceMessenger.Default.SendV<HashSetting, HashFormat>(new(this, value), EMT.HashSetting_Format);

    /// <summary>
    /// 是否选择该项.
    /// </summary>
    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsChecked);

    /// <summary>
    /// 是否在软件内启用该项.
    /// </summary>
    [ObservableProperty]
    private bool _isEnabledForApp;

    partial void OnIsEnabledForAppChanged(bool value) =>
        WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsEnabledForApp);

    /// <summary>
    /// 是否在资源管理器上下文菜单内启用该项.
    /// </summary>
    [ObservableProperty]
    private bool _isEnabledForContextMenu;

    partial void OnIsEnabledForContextMenuChanged(bool value) =>
        WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsEnabledForContextMenu);
}
