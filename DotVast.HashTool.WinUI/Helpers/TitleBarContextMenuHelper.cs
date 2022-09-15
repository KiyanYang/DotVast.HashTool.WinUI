using System.Runtime.InteropServices;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

internal class TitleBarContextMenuHelper
{
    private enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    };

    [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr SetPreferredAppMode(PreferredAppMode preferredAppMode);

    /// <summary>
    /// 设置标题栏上下文菜单主题为"允许深色".
    /// </summary>
    /// <returns></returns>
    public static IntPtr SetTitleBarContextMenuAllowDark() =>
        SetPreferredAppMode(PreferredAppMode.AllowDark);

    // TODO: SetPreferredAppMode 方法仅首次调用有效.
    //private static IntPtr UpdateTitleBarContextMenu(ElementTheme theme)
    //{
    //    var mode = theme switch
    //    {
    //        ElementTheme.Light => PreferredAppMode.ForceLight,
    //        ElementTheme.Dark => PreferredAppMode.ForceDark,
    //        _ => PreferredAppMode.AllowDark,
    //    };
    //    return SetPreferredAppMode(mode);
    //}
}
