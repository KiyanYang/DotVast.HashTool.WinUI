using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Controls;

public sealed class SettingItem : ContentControl
{
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

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
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
        UpdateIconState();
        UpdateDescriptionState();
        base.OnApplyTemplate();
    }

    private void UpdateIconState()
    {
        var state = Icon == null ? "NoIconState" : "IconState";
        VisualStateManager.GoToState(this, state, true);
    }

    private void UpdateDescriptionState()
    {
        var state = string.IsNullOrEmpty(Description) ? "NoDescriptionState" : "DescriptionState";
        VisualStateManager.GoToState(this, state, true);
    }
}
