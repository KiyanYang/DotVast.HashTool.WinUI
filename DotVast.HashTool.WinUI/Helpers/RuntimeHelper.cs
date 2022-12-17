using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Helpers;

public sealed class RuntimeHelper
{
    #region AppVersion

    private static Version? s_appVersion;
    public static Version AppVersion => s_appVersion ??= GetAppVersion();
    private static Version GetAppVersion()
    {
        var packageVersion = Package.Current.Id.Version;
        return new(packageVersion.Major, packageVersion.Minor, packageVersion.Build); // 使用语义化版本
    }

    #endregion AppVersion
}
