// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class XamlFunc
{
    public static bool TrueIfTrue(bool? b) =>
        b == true;

    public static object TrueValueIfTrue(bool? b, object trueValue, object falseValue) =>
        b == true ? trueValue : falseValue;

    public static Visibility VisibleIfTrue(bool? b) =>
        b == true ? Visibility.Visible : Visibility.Collapsed;
}
