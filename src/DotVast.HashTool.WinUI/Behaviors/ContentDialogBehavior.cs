// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Behaviors;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace DotVast.HashTool.WinUI.Behaviors;

/// <summary>
/// 使 ContentDialog 的 SmokeLayerBackground 覆盖标题栏.
/// </summary>
internal sealed class ContentDialogBehavior : BehaviorBase<FrameworkElement>
{
    protected override void OnAssociatedObjectLoaded()
    {
        var parent = (FrameworkElement)VisualTreeHelper.GetParent(AssociatedObject);
        var smokeLayerBackground = parent.FindDescendant<Rectangle>(e => e.Name == "SmokeLayerBackground")!;
        smokeLayerBackground.Margin = new Thickness(0);
        smokeLayerBackground.RegisterPropertyChangedCallback(FrameworkElement.MarginProperty, OnMarginChanged);
    }

    private static void OnMarginChanged(DependencyObject sender, DependencyProperty property)
    {
        if (property == FrameworkElement.MarginProperty)
        {
            sender.ClearValue(property);
        }
    }
}
