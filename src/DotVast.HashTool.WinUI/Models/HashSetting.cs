using System.Text.Json.Serialization;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashSetting : ObservableObject
{
    public HashKind Kind
    {
        get => _kind;
        init
        {
            _kind = value;
            Name = App.GetService<IHashService>().GetHashData(value).Name;
        }
    }
    private HashKind _kind;

    [JsonIgnore]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// 是否选择该项.
    /// </summary>
    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashSettingIsCheckedChangedMessage(this, value));

    /// <summary>
    /// 是否在软件内启用该项.
    /// </summary>
    [ObservableProperty]
    private bool _isEnabledForApp;

    partial void OnIsEnabledForAppChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashSettingIsEnabledForAppChangedMessage(this, value));

    /// <summary>
    /// 是否在资源管理器上下文菜单内启用该项.
    /// </summary>
    [ObservableProperty]
    private bool _isEnabledForContextMenu;

    partial void OnIsEnabledForContextMenuChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new HashSettingIsEnabledForContextMenuChangedMessage(this, value));
}
