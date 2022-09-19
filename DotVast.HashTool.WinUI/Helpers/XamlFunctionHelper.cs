using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class XamlFunctionHelper
{
    public static Visibility VisibleIfText(HashTaskMode mode) =>
        mode == HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;

    public static Visibility VisibleIfNotText(HashTaskMode mode) =>
        mode != HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;
}
