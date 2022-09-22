namespace DotVast.HashTool.WinUI.Models.Records;

public sealed record LicenseInfo(string Name, string License, string Url);

public sealed class License
{
    public static readonly string Unknown = "License";
    public static readonly string Apache_2_0 = "Apache License 2.0";
    public static readonly string MIT = "MIT License";
}
