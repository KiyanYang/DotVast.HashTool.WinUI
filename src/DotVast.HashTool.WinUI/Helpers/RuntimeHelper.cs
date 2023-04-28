// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.Helpers;

public sealed class RuntimeHelper
{
    static RuntimeHelper()
    {
        ThreadPool.GetMaxThreads(out s_defaultMaxWorkerThreads, out s_defaultMaxCompletionPortThreads);
        ThreadPool.GetMinThreads(out s_defaultMinWorkerThreads, out s_defaultMinCompletionPortThreads);
    }

    #region AppVersion

    private static Version? s_appVersion;
    public static Version AppVersion => s_appVersion ??= GetAppVersion();
    private static Version GetAppVersion()
    {
        var packageVersion = Package.Current.Id.Version;
        return new(packageVersion.Major, packageVersion.Minor, packageVersion.Build); // use Semantic Versioning
    }

    #endregion AppVersion

    #region Threads

    public static int DefaultMaxWorkerThreads => s_defaultMaxWorkerThreads;
    private static readonly int s_defaultMaxWorkerThreads;

    public static int DefaultMaxCompletionPortThreads => s_defaultMaxCompletionPortThreads;
    private static readonly int s_defaultMaxCompletionPortThreads;

    public static int DefaultMinWorkerThreads => s_defaultMinWorkerThreads;
    private static readonly int s_defaultMinWorkerThreads;

    public static int DefaultMinCompletionPortThreads => s_defaultMinCompletionPortThreads;
    private static readonly int s_defaultMinCompletionPortThreads;

    #endregion Threads
}
