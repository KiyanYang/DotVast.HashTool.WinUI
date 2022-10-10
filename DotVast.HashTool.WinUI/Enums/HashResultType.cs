using DotVast.HashTool.WinUI.Core.Enums;

namespace DotVast.HashTool.WinUI.Enums;

public sealed class HashResultType : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static readonly HashResultType File = new(Localization.HashResultType_File);

    /// <summary>
    /// 文本.
    /// </summary>
    public static readonly HashResultType Text = new(Localization.HashResultType_Text);

    private HashResultType(string name) : base(name) { }
}
