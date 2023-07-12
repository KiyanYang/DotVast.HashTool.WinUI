// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class JsonExportResolver : IExportResolver
{
    public bool CanResolve(ExportKind exportKind, object obj)
    {
        return exportKind == (ExportKind.Json | ExportKind.HashTask)
            && obj is IEnumerable<HashTask>;
    }

    public async Task ExportAsync(string filePath, ExportKind exportKind, object obj)
    {
        if (!CanResolve(exportKind, obj))
        {
            throw new InvalidOperationException();
        }

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
        };
        //options.Converters.Add(new HashResultItemConverter());

        var hashTasks = (IEnumerable<HashTask>)obj;
        using var stream = File.Create(filePath);

        if (hashTasks.Take(2).Count() == 1)
        {
            await JsonSerializer.SerializeAsync(stream, hashTasks.First(), new JsonContext(options).HashTask);
        }
        else
        {
            await JsonSerializer.SerializeAsync(stream, hashTasks, new JsonContext(options).IEnumerableHashTask);
        }
    }

    //sealed class HashResultItemConverter : JsonConverter<HashResultItem>
    //{
    //    public override HashResultItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
    //        throw new NotImplementedException();

    //    public override void Write(Utf8JsonWriter writer, HashResultItem value, JsonSerializerOptions options)
    //    {
    //        var hashKindJsonConverter = (JsonConverter<HashKind>)options.GetConverter(typeof(HashKind));
    //        writer.WriteStartObject();
    //        writer.WritePropertyName(nameof(HashResultItem.Option.Kind));
    //        hashKindJsonConverter.Write(writer, value.Option.Kind, options);
    //        writer.WriteString(nameof(HashResultItem.Value), value.Value);
    //        writer.WriteEndObject();
    //    }
    //}
}
