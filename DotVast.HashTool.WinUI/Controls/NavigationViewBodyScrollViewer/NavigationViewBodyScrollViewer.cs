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
