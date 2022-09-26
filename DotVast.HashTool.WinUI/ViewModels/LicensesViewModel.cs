using DotVast.HashTool.WinUI.Models.Records;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class LicensesViewModel : ObservableRecipient
{
    public LicenseInfo[] Licenses
    {
        get;
    } = new LicenseInfo[]
    {
        new("CommunityToolkit.MVVM", License.MIT, "https://licenses.nuget.org/MIT"),
        new("HashLib4CSharp", License.MIT, "https://www.nuget.org/packages/HashLib4CSharp/1.5.0/license"),
        new("Microsoft.Extensions.Hosting", License.MIT, "https://licenses.nuget.org/MIT"),
        new("Microsoft.WindowsAppSDK", License.Unknown, "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.1.5/license"),
        new("Microsoft.Xaml.Behaviors.WinUI.Managed", License.MIT, "https://licenses.nuget.org/MIT"),
        new("Serilog.Extensions.Hosting", License.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("Serilog.Sinks.File", License.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("WinUIEx", License.Apache_2_0, "https://licenses.nuget.org/Apache-2.0"),
        new("QuickXorHash.cs", License.MIT, "https://gist.github.com/rgregg/c07a91964300315c6c3e77f7b5b861e4"),
        new("DotVast.Toolkit.StringResource", License.MIT, "https://github.com/KiyanYang/DotVast.Toolkit.StringResource/blob/main/LICENSE.txt"),
    };
}
