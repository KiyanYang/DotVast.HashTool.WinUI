using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Helpers;

internal static class PathHelper
{
    private static string? s_appDataLocalPhysicalPath;

    public static string AppDataLocalPhysicalPath =>
        s_appDataLocalPhysicalPath ??= GetAppDataLocalPhysicalPath();

    /// <summary>
    /// 获取软件本地缓存文件夹（Environment.SpecialFolder.LocalApplicationData）的物理路径。
    /// </summary>
    /// <returns>本地缓存文件夹的物理路径。</returns>
    private static string GetAppDataLocalPhysicalPath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var packageFamilyName = Package.Current.Id.FamilyName;
        return Path.Combine(localAppData, "Packages", packageFamilyName, "LocalCache/Local");
    }
}
