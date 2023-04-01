using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Controls;

public sealed class NavigationViewBodyScrollViewer : ContentControl
{
    private const string PartScrollViewer = "Decoration";

    private ScrollViewer? _scrollViewer;

    public NavigationViewBodyScrollViewer()
    {
        DefaultStyleKey = typeof(NavigationViewBodyScrollViewer);
    }

    #region Dependency Properties

    public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(
        nameof(HorizontalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(NavigationViewBodyScrollViewer),
        new PropertyMetadata(null));

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

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
