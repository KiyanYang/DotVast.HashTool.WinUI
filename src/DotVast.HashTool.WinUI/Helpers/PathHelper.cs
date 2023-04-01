using Windows.Storage;

namespace DotVast.HashTool.WinUI.Helpers;

internal static class PathHelper
{
    private static string? s_appDataLocalPhysicalPath;

    /// <summary>
    /// 本地缓存文件夹的物理路径。
    /// </summary>
    /// <remarks>
    /// 相关文档: <see href="https://learn.microsoft.com/windows/msix/desktop/desktop-to-uwp-behind-the-scenes">Understanding how packaged desktop apps run on Windows</see>
    /// </remarks>
    public static string AppDataLocalPhysicalPath =>
        s_appDataLocalPhysicalPath ??= GetAppDataLocalPhysicalPath();

    /// <summary>
    /// 获取软件本地缓存文件夹（Environment.SpecialFolder.LocalApplicationData）的物理路径。
    /// </summary>
    /// <returns>本地缓存文件夹的物理路径。</returns>
    private static string GetAppDataLocalPhysicalPath()
    {
        return Path.GetFullPath("Local", ApplicationData.Current.LocalCacheFolder.Path);
    }
}
