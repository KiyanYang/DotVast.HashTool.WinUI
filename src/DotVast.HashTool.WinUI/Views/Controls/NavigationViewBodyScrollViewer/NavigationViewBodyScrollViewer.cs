// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views.Controls;

public sealed partial class NavigationViewBodyScrollViewer : ContentControl
{
    private const string PartScrollViewer = "Decoration";

    private ScrollViewer? _scrollViewer;

    public NavigationViewBodyScrollViewer()
    {
        DefaultStyleKey = typeof(NavigationViewBodyScrollViewer);
    }

    #region Dependency Properties

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

    public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(
        nameof(HorizontalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(NavigationViewBodyScrollViewer),
        new PropertyMetadata(null));

    #endregion Dependency Properties

    protected override void OnApplyTemplate()
    {
        _scrollViewer = GetTemplateChild(PartScrollViewer) as ScrollViewer;
        base.OnApplyTemplate();
    }

    public bool ChangeView(double? horizontalOffset, double? verticalOffset, float? zoomFactor)
    {
        return _scrollViewer!.ChangeView(horizontalOffset, verticalOffset, zoomFactor);
    }
}
