using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTask : ObservableObject
{
    /// <summary>
    /// 任务创建时间.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// 用时.
    /// </summary>
    [ObservableProperty]
    private TimeSpan _elapsed;

    /// <summary>
    /// 任务状态.
    /// </summary>
    [ObservableProperty]
    private HashTaskState? _state;

    public HashTaskMode Mode { get; set; } = HashTaskMode.File;

    public string Content { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(EncodingJsonConverter))]
    public Encoding? Encoding { get; set; }

    public IList<Hash> SelectedHashs { get; init; } = Array.Empty<Hash>();

    /// <summary>
    /// 结果.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HashResult>? _results;

    private sealed class EncodingJsonConverter : JsonConverter<Encoding?>
    {
        public override Encoding? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var name = reader.GetString();
            return Encoding.GetEncodings()
                           .FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.OrdinalIgnoreCase))?
                           .GetEncoding();
        }

        public override void Write(Utf8JsonWriter writer, Encoding? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.WebName);
        }
    }
}
