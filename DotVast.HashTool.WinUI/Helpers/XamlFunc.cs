using DotVast.HashTool.WinUI.Enums;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class XamlFunc
{
    public static bool TrueIfTrue(bool? b) =>
        b == true;

    public static object TrueValueIfTrue(bool? b, object trueValue, object falseValue) =>
        b == true ? trueValue : falseValue;

    public static Visibility VisibleIfText(HashTaskMode mode) =>
        mode == HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;

    public static Visibility VisibleIfNotText(HashTaskMode mode) =>
        mode != HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;
}
