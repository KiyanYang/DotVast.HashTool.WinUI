namespace DotVast.HashTool.WinUI.Enums;

public sealed class HashTaskMode : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static HashTaskMode File { get; } = new(Localization.HashTaskMode_File);

    /// <summary>
    /// 文件夹.
    /// </summary>
    public static HashTaskMode Folder { get; } = new(Localization.HashTaskMode_Folder);

    /// <summary>
    /// 文本.
    /// </summary>
    public static HashTaskMode Text { get; } = new(Localization.HashTaskMode_Text);

    private HashTaskMode(string name) : base(name) { }
}
