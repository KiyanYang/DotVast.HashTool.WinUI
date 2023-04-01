using System.Runtime.InteropServices;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

internal sealed partial class TitleBarContextMenuHelper
{
    private enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    };

    [LibraryImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true)]
    private static partial IntPtr SetPreferredAppMode(PreferredAppMode preferredAppMode);

    [LibraryImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true)]
    private static partial IntPtr FlushMenuThemes();

    /// <summary>
    /// 更新标题栏上下文菜单主题.
    /// </summary>
    /// <param name="theme">元素主题.</param>
    public static void UpdateTitleBarContextMenu(ElementTheme theme)
    {
        var mode = theme switch
        {
            ElementTheme.Light => PreferredAppMode.ForceLight,
            ElementTheme.Dark => PreferredAppMode.ForceDark,
            _ => PreferredAppMode.AllowDark,
        };
        SetPreferredAppMode(mode);
        FlushMenuThemes();
    }
}
