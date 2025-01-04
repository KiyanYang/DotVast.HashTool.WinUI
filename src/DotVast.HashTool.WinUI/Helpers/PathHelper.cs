// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

using Windows.Storage;

namespace DotVast.HashTool.WinUI.Helpers;

internal static class PathHelper
{
    /// <summary>
    /// 本地缓存文件夹的物理路径。
    /// </summary>
    /// <remarks>
    /// 相关文档: <see href="https://learn.microsoft.com/windows/msix/desktop/desktop-to-uwp-behind-the-scenes">Understanding how packaged desktop apps run on Windows</see>
    /// </remarks>
    [field: AllowNull]
    public static string AppDataLocalPhysicalPath => field ??= GetAppDataLocalPhysicalPath();

    /// <summary>
    /// 获取软件本地缓存文件夹（Environment.SpecialFolder.LocalApplicationData）的物理路径。
    /// </summary>
    /// <returns>本地缓存文件夹的物理路径。</returns>
    private static string GetAppDataLocalPhysicalPath()
    {
        return Path.GetFullPath("Local", ApplicationData.Current.LocalCacheFolder.Path);
    }
}
