using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Controls;

public sealed class SettingItem : ContentControl
{
    private SettingItem? _settingItem;
    private ContentPresenter? _contentPresenter;

    public SettingItem()
    {
        DefaultStyleKey = typeof(SettingItem);
    }

    #region Dependency Properties

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(SettingItem),
        new PropertyMetadata(null, (d, e) => ((SettingItem)d).UpdateIconState()));

    public static readonly DependencyProperty ShowPlaceholderIconProperty = DependencyProperty.Register(
        nameof(ShowPlaceholderIcon),
        typeof(bool),
        typeof(SettingItem),
        new PropertyMetadata(false, (d, e) => ((SettingItem)d).UpdateIconColumnState()));

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
       nameof(Header),
       typeof(string),
       typeof(SettingItem),
       new PropertyMetadata(null));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(null, (d, e) => ((SettingItem)d).UpdateDescriptionState()));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool ShowPlaceholderIcon
    {
        get => (bool)GetValue(ShowPlaceholderIconProperty);
        set => SetValue(ShowPlaceholderIconProperty, value);
    }

    [Localizable(true)]
    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    [Localizable(true)]
    public string? Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    #endregion Dependency Properties

    protected override void OnApplyTemplate()
    {
        SizeChanged -= SettingItem_SizeChanged;
        _settingItem = this;
        _contentPresenter = (ContentPresenter)_settingItem.GetTemplateChild("ContentPresenter");
        UpdateIconState();
        UpdateIconColumnState();
        UpdateDescriptionState();
        SizeChanged += SettingItem_SizeChanged;
        base.OnApplyTemplate();
    }

    private void SettingItem_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateIconColumnState();

        if (_settingItem != null && _contentPresenter != null)
        {
            _contentPresenter.MaxWidth = _settingItem.ActualWidth * 0.6;
        }
    }

    private void UpdateIconState()
    {
        var state = Icon == null ? "NoIconState" : "IconState";
        VisualStateManager.GoToState(this, state, true);
    }

    private void UpdateIconColumnState()
    {
        var state = (Icon, ShowPlaceholderIcon, App.MainWindow.Width) switch
        {
            (null, false, _) or (_, _, <= 640) => "NoIconColumnState",
            _ => "IconColumnState",
        };
        VisualStateManager.GoToState(this, state, true);
    }

    private void UpdateDescriptionState()
    {
        var state = string.IsNullOrEmpty(Description) ? "NoDescriptionState" : "DescriptionState";
        VisualStateManager.GoToState(this, state, true);
    }
}
