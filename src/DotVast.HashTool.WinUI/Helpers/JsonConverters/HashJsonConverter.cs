using DotVast.HashTool.WinUI.Core.Enums;
using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Helpers.JsonConverters;

public sealed class HashJsonConverter : JsonConverterForGenericEnum<Hash, string>
{
    public HashJsonConverter() : base(JsonContext.Default.Options, StringComparer.OrdinalIgnoreCase) { }
}
