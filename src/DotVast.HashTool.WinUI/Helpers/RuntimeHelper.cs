// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Helpers;

public sealed class RuntimeHelper
{
    #region AppVersion

    [field: AllowNull]
    public static Version AppVersion => field ??= GetAppVersion();
    private static Version GetAppVersion()
    {
        var packageVersion = Package.Current.Id.Version;
        return new(packageVersion.Major, packageVersion.Minor, packageVersion.Build); // use Semantic Versioning
    }

    #endregion AppVersion
}
