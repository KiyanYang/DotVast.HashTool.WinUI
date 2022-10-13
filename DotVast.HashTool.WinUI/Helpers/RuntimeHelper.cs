using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Helpers;

public sealed class RuntimeHelper
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    #region IsMSIX

    private static bool? s_isMSIX;
    public static bool IsMSIX => s_isMSIX ??= GetIsMSIX();
    private static bool GetIsMSIX()
    {
        var length = 0;
        return GetCurrentPackageFullName(ref length, null) != 15700L;
    }

    #endregion IsMSIX

    #region AppVersion

    private static Version? s_appVersion;
    public static Version AppVersion => s_appVersion ??= GetAppVersion();
    private static Version GetAppVersion()
    {
        if (IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;
            // 使用语义化版本
            s_appVersion = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build);
        }
        else
        {
            s_appVersion = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return s_appVersion;
    }

    #endregion AppVersion
}
