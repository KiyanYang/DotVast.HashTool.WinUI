using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Core.Enums;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonConverterFactoryForGenericEnumDerived))]
public sealed class HashResultType : GenericEnum<string>
{
    /// <summary>
    /// 文件.
    /// </summary>
    public static readonly HashResultType File = new(LocalizationEnum.HashResultType_File);

    /// <summary>
    /// 文本.
    /// </summary>
    public static readonly HashResultType Text = new(LocalizationEnum.HashResultType_Text);

    private HashResultType(string name) : base(name) { }
}
