// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Enums;

public enum AppTheme
{
    Default,
    Light,
    Dark,
}

public static class AppThemeExtensions
{
    public static string ToDisplay(this AppTheme appTheme)
    {
        return appTheme switch
        {
            AppTheme.Default => LocalizationEnum.AppTheme_Default,
            AppTheme.Light => LocalizationEnum.AppTheme_Light,
            AppTheme.Dark => LocalizationEnum.AppTheme_Dark,
            _ => throw new ArgumentOutOfRangeException(nameof(appTheme)),
        };
    }

    public static ElementTheme ToElementTheme(this AppTheme appTheme)
    {
        return appTheme switch
        {
            AppTheme.Default => ElementTheme.Default,
            AppTheme.Light => ElementTheme.Light,
            AppTheme.Dark => ElementTheme.Dark,
            _ => throw new ArgumentOutOfRangeException(nameof(appTheme)),
        };
    }
}
