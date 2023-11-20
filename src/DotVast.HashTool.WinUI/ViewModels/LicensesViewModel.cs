// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed class LicensesViewModel : IViewModel
{
    public LicenseInfo[] Licenses { get; } =
    [
        new("crates/blake3", License.Apache_2_0, "https://github.com/BLAKE3-team/BLAKE3/blob/master/LICENSE", "https://github.com/BLAKE3-team/BLAKE3"),
        new("crates/md5", License.MIT, "https://github.com/RustCrypto/hashes/blob/master/md5/LICENSE-MIT", "https://github.com/RustCrypto/hashes"),
        new("crates/ripemd", License.MIT, "https://github.com/RustCrypto/hashes/blob/master/ripemd/LICENSE-MIT", "https://github.com/RustCrypto/hashes"),
        new("crates/sha1", License.MIT, "https://github.com/RustCrypto/hashes/blob/master/sha1/LICENSE-MIT", "https://github.com/RustCrypto/hashes"),
        new("crates/sm3", License.MIT, "https://github.com/RustCrypto/hashes/blob/master/sm3/LICENSE-MIT", "https://github.com/RustCrypto/hashes"),

        new("CommunityToolkit.Mvvm", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.Mvvm"),
        new("CommunityToolkit.WinUI.Behaviors", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.Behaviors"),
        new("CommunityToolkit.WinUI.Controls.Primitives", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.Controls.Primitives"),
        new("CommunityToolkit.WinUI.Controls.SettingsControls", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.Controls.SettingsControls"),
        new("CommunityToolkit.WinUI.UI.Controls.Markdown", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/CommunityToolkit.WinUI.UI.Controls.Markdown"),

        new("DotVast.Toolkit.StringResource", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/DotVast.Toolkit.StringResource"),

        new("HashLib4CSharp", License.MIT, "https://www.nuget.org/packages/HashLib4CSharp/1.5.0/License", "https://www.nuget.org/packages/HashLib4CSharp"),

        new("Microsoft.Extensions.Hosting", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/Microsoft.Extensions.Hosting"),
        new("Microsoft.Extensions.Http", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/Microsoft.Extensions.Http"),
        new("Microsoft.WindowsAppSDK", License.Unknown, "https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.2.230118.102/License", "https://www.nuget.org/packages/Microsoft.WindowsAppSDK"),

        new("Serilog", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog"),
        new("Serilog.Extensions.Hosting", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog.Extensions.Hosting"),
        new("Serilog.Sinks.File", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/Serilog.Sinks.File"),

        new("System.IO.Hashing", License.MIT, NugetLicenseUrl.MIT, "https://www.nuget.org/packages/System.IO.Hashing"),

        new("WinUIEx", License.Apache_2_0, NugetLicenseUrl.Apache_2_0, "https://www.nuget.org/packages/WinUIEx"),
    ];
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
