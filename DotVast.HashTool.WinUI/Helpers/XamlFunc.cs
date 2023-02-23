using DotVast.HashTool.WinUI.Enums;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class XamlFunc
{
    public static bool TrueIfTrue(object obj) =>
        (obj as bool?) == true;

    public static Visibility VisibleIfText(HashTaskMode mode) =>
        mode == HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;

    public static Visibility VisibleIfNotText(HashTaskMode mode) =>
        mode != HashTaskMode.Text ? Visibility.Visible : Visibility.Collapsed;
}
