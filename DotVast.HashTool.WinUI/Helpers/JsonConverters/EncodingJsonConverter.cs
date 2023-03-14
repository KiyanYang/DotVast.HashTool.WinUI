using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Helpers.JsonConverters;

internal sealed class EncodingJsonConverter : JsonConverter<Encoding?>
{
    public override Encoding? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var name = reader.GetString();
        return TextEncoding.EncodingInfos
            .FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.OrdinalIgnoreCase))
            ?.GetEncoding();
    }

    public override void Write(Utf8JsonWriter writer, Encoding? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.WebName);
    }
}
