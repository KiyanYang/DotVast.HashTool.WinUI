using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Core.Enums;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonConverterFactoryForGenericEnumDerived))]
public sealed class HashTaskMode : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static readonly HashTaskMode File = new(LocalizationEnum.HashTaskMode_File);

    /// <summary>
    /// 文件夹.
    /// </summary>
    public static readonly HashTaskMode Folder = new(LocalizationEnum.HashTaskMode_Folder);

    /// <summary>
    /// 文本.
    /// </summary>
    public static readonly HashTaskMode Text = new(LocalizationEnum.HashTaskMode_Text);

    private HashTaskMode(string name) : base(name) { }
}
