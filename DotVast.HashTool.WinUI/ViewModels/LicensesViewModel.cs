using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class LicensesViewModel : ObservableRecipient
{
    public LicenseInfo[] Licenses
    {
        get;
    } = new LicenseInfo[]
    {
        new("CommunityToolkit.MVVM", LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("CommunityToolkit.WinUI.UI", LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("CommunityToolkit.WinUI.UI.Behaviors", LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("CommunityToolkit.WinUI.UI.Controls.Markdown",LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("CryptoBase", LicenseType.MIT, "https://github.com/HMBSbige/CryptoBase/blob/1.7.2/LICENSE"),
        new("DotVast.Toolkit.StringResource", LicenseType.MIT, "https://github.com/KiyanYang/DotVast.Toolkit.StringResource/blob/main/LICENSE.txt"),
        new("HashLib4CSharp", LicenseType.MIT, "https://www.nuget.org/packages/HashLib4CSharp/1.5.0/license"),
        new("Microsoft.Extensions.Hosting", LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("Microsoft.Extensions.Http", LicenseType.MIT, "https://licenses.nuget.org/MIT"),
        new("Microsoft.WindowsAppSDK", LicenseType.Unknown, "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.1.5/license"),
        new("Serilog.Extensions.Hosting", LicenseType.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("Serilog.Sinks.File", LicenseType.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("WinUIEx", LicenseType.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("QuickXorHash.cs", LicenseType.MIT, "https://gist.github.com/rgregg/c07a91964300315c6c3e77f7b5b861e4"),
    };
}
