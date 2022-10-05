namespace DotVast.HashTool.WinUI.Enums;

public sealed class HashResultType : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static HashResultType File { get; } = new(Localization.HashResultType_File);

    /// <summary>
    /// 文本.
    /// </summary>
    public static HashResultType Text { get; } = new(Localization.HashResultType_Text);

    private HashResultType(string name) : base(name) { }
}
