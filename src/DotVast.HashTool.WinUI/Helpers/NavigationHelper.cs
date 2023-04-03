using DotVast.HashTool.WinUI.Enums;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Helpers;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="MainPage" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, PageKey.MainPage);
public sealed class NavigationHelper
{
    public static PageKey GetNavigateTo(NavigationViewItem item) =>
        (PageKey)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, PageKey value) =>
        item.SetValue(NavigateToProperty, value);

    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(PageKey), typeof(NavigationHelper), new PropertyMetadata(null));
}
