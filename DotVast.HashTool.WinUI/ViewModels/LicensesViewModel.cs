using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed class LicensesViewModel : ObservableObject
{
    public LicenseInfo[] Licenses { get; } = new LicenseInfo[]
    {
        new("CommunityToolkit.Labs.WinUI", License.MIT, "https://github.com/CommunityToolkit/Labs-Windows/blob/main/License.md", "https://github.com/CommunityToolkit/Labs-Windows"),
        new("CommunityToolkit.Mvvm", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.Mvvm"),
        new("CommunityToolkit.WinUI.UI", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.UI"),
        new("CommunityToolkit.WinUI.UI.Behaviors", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.UI.Behaviors"),
        new("CommunityToolkit.WinUI.UI.Controls.Markdown", License.MIT, NugetLicenseUrl.MIT,"https://www.nuget.org/packages/CommunityToolkit.WinUI.UI.Controls.Markdown"),

        new("CryptoBase", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CryptoBase"),

        new("DotVast.Toolkit.StringResource", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/DotVast.Toolkit.StringResource"),

        new("HashLib4CSharp", License.MIT, "https://www.nuget.org/packages/HashLib4CSharp/1.5.0/License", "https://www.nuget.org/packages/HashLib4CSharp"),

        new("Microsoft.Extensions.Hosting", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/Microsoft.Extensions.Hosting"),
        new("Microsoft.Extensions.Http", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/Microsoft.Extensions.Http"),
        new("Microsoft.WindowsAppSDK", License.Unknown, "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.2.230118.102/License","https://www.nuget.org/packages/Microsoft.WindowsAppSDK"),

        new("Serilog", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog"),
        new("Serilog.Extensions.Hosting", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog.Extensions.Hosting"),
        new("Serilog.Sinks.File", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog.Sinks.File"),

        new("System.IO.Hashing", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/System.IO.Hashing"),

        new("WinUIEx", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/WinUIEx"),

        new("QuickXorHash.cs", License.MIT, "https://gist.github.com/rgregg/c07a91964300315c6c3e77f7b5b861e4", "https://gist.github.com/rgregg/c07a91964300315c6c3e77f7b5b861e4"),
    };
}

static file class License
{
    public static readonly string Unknown = "License";
    public static readonly string Apache_2_0 = "Apache License 2.0";
    public static readonly string MIT = "MIT License";
}

static file class NugetLicenseUrl
{
    public static readonly string Apache_2_0 = "https://licenses.nuget.org/Apache-2.0";
    public static readonly string MIT = "https://licenses.nuget.org/MIT";
}
