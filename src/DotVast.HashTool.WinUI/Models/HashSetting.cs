using System.ComponentModel;
using System.Text.Json.Serialization;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashSetting : ObservableObject
{
    public HashKind Kind
    {
        get => _kind;
        set // TODO: json source generator does not support `init` in .Net7
        {
            _kind = value;
            Name = _kind.ToName();
        }
    }
    private HashKind _kind;

    /// <summary>
    /// 名称. 用作界面显示, 尽可能避免内部调用.
    /// </summary>
    [JsonIgnore]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// 哈希输出格式.
    /// </summary>
    public HashFormat Format
    {
        get => _format;
        set
        {
            if (_format != value)
            {
                OnPropertyChanging(s_formatChangingEventArgs);
                _format = value;
                OnPropertyChanged(s_formatChangedEventArgs);
                WeakReferenceMessenger.Default.SendV<HashSetting, HashFormat>(new(this, value), EMT.HashSetting_Format);
            }
        }
    }
    private HashFormat _format;
    private static readonly PropertyChangingEventArgs s_formatChangingEventArgs = new(nameof(Format));
    private static readonly PropertyChangedEventArgs s_formatChangedEventArgs = new(nameof(Format));

    /// <summary>
    /// 是否选择该项.
    /// </summary>
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                OnPropertyChanging(s_isCheckedChangingEventArgs);
                _isChecked = value;
                OnPropertyChanged(s_isCheckedChangedEventArgs);
                WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsChecked);
            }
        }
    }
    private bool _isChecked;
    private static readonly PropertyChangingEventArgs s_isCheckedChangingEventArgs = new(nameof(IsChecked));
    private static readonly PropertyChangedEventArgs s_isCheckedChangedEventArgs = new(nameof(IsChecked));

    /// <summary>
    /// 是否在软件内启用该项.
    /// </summary>
    public bool IsEnabledForApp
    {
        get => _isEnabledForApp;
        set
        {
            if (_isEnabledForApp != value)
            {
                OnPropertyChanging(s_isEnabledForAppChangingEventArgs);
                _isEnabledForApp = value;
                OnPropertyChanged(s_isEnabledForAppChangedEventArgs);
                WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsEnabledForApp);
            }
        }
    }
    private bool _isEnabledForApp;
    private static readonly PropertyChangingEventArgs s_isEnabledForAppChangingEventArgs = new(nameof(IsEnabledForApp));
    private static readonly PropertyChangedEventArgs s_isEnabledForAppChangedEventArgs = new(nameof(IsEnabledForApp));

    /// <summary>
    /// 是否在资源管理器上下文菜单内启用该项.
    /// </summary>
    public bool IsEnabledForContextMenu
    {
        get => _isEnabledForContextMenu;
        set
        {
            if (_isEnabledForContextMenu != value)
            {
                OnPropertyChanging(s_isEnabledForContextMenuChangingEventArgs);
                _isEnabledForContextMenu = value;
                OnPropertyChanged(s_isEnabledForContextMenuChangedEventArgs);
                WeakReferenceMessenger.Default.SendV<HashSetting, bool>(new(this, value), EMT.HashSetting_IsEnabledForContextMenu);
            }
        }
    }
    private bool _isEnabledForContextMenu;
    private static readonly PropertyChangingEventArgs s_isEnabledForContextMenuChangingEventArgs = new(nameof(IsEnabledForContextMenu));
    private static readonly PropertyChangedEventArgs s_isEnabledForContextMenuChangedEventArgs = new(nameof(IsEnabledForContextMenu));
}
