using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Helpers.JsonConverters;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Helpers;

/// <summary>
/// 公用 JsonSerializerContext.
/// </summary>
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(TimeSpan))]

[JsonSerializable(typeof(IList<Hash>))]
[JsonSerializable(typeof(HashTaskMode))]
[JsonSerializable(typeof(HashTaskState))]

[JsonSerializable(typeof(GitHubRelease))]
[JsonSerializable(typeof(GitHubRelease[]))]
[JsonSerializable(typeof(ObservableCollection<HashResult>))]
public sealed partial class JsonContext : JsonSerializerContext
{
}

// 源生成器不能读取其他源生成器生成的内容, 因此需要自己手动构建.
// 下面的代码积极地从源生成器生成的内容复制.
// 主要修改内容如下:
//  根据实际需要增删一些 JsonPropertyInfo, 同时相应地更改其容器 JsonPropertyInfo[] 的长度.
//  对于 [ObservableProperty] 标注的字段
//      要在 JsonPropertyInfoValues 构造中修改属性 IsProperty, IsPublic, Getter, Setter, PropertyName,
//      此外还应注意 Converter, IgnoreCondition 等其他的属性是否需要修改.
//  对于末端类型(即不再进行展开的类型)
//      参考 [JsonSerializable(typeof(bool))] 生成的内容进行编写
//      例如下方的 JsonTypeInfo<Encoding>.

public sealed partial class JsonContext2 : JsonSerializerContext
{
    private static JsonSerializerOptions s_defaultOptions { get; } = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        IncludeFields = false,
        WriteIndented = false,
    };

    private static JsonContext2? s_defaultContext;

    /// <summary>
    /// The default <see cref="JsonSerializerContext"/> associated with a default <see cref="JsonSerializerOptions"/> instance.
    /// </summary>
    public static JsonContext2 Default => s_defaultContext ??= new JsonContext2(new JsonSerializerOptions(s_defaultOptions));

    /// <summary>
    /// The source-generated options associated with this context.
    /// </summary>
    protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = s_defaultOptions;

    /// <inheritdoc/>
    public JsonContext2() : base(null)
    {
    }

    /// <inheritdoc/>
    public JsonContext2(JsonSerializerOptions options) : base(options)
    {
    }

    private static JsonConverter? GetRuntimeProvidedCustomConverter(JsonSerializerOptions options, Type type)
    {
        IList<JsonConverter> converters = options.Converters;

        for (int i = 0; i < converters.Count; i++)
        {
            JsonConverter? converter = converters[i];

            if (converter.CanConvert(type))
            {
                if (converter is JsonConverterFactory factory)
                {
                    converter = factory.CreateConverter(type, options);
                    if (converter == null || converter is JsonConverterFactory)
                    {
                        throw new InvalidOperationException(string.Format("The converter '{0}' cannot return null or a JsonConverterFactory instance.", factory.GetType()));
                    }
                }

                return converter;
            }
        }

        return null;
    }

    private static JsonConverter GetConverterFromFactory(JsonSerializerOptions options, Type type, JsonConverterFactory factory)
    {
        JsonConverter? converter = factory.CreateConverter(type, options);
        if (converter == null || converter is JsonConverterFactory)
        {
            throw new InvalidOperationException(string.Format("The converter '{0}' cannot return null or a JsonConverterFactory instance.", factory.GetType()));
        }

        return converter;
    }
}

public sealed partial class JsonContext2 : IJsonTypeInfoResolver
{
    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type)
    {
        if (type == typeof(HashTask))
        {
            return HashTask;
        }

        if (type == typeof(IEnumerable<HashTask>))
        {
            return IEnumerableHashTask;
        }

        if (type == typeof(Encoding))
        {
            return Encoding;
        }

        return JsonContext.Default.GetTypeInfo(type)!;
    }

    JsonTypeInfo? IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        if (type == typeof(HashTask))
        {
            return Create_HashTask(options, makeReadOnly: false);
        }

        if (type == typeof(IEnumerable<HashTask>))
        {
            return Create_IEnumerableHashTask(options, makeReadOnly: false);
        }

        if (type == typeof(Encoding))
        {
            return Create_Encoding(options, makeReadOnly: false);
        }

        return ((IJsonTypeInfoResolver)JsonContext.Default).GetTypeInfo(type, options);
    }
}

#region Models.HashTask
public sealed partial class JsonContext2
{
    private JsonTypeInfo<HashTask>? _hashTask;
    public JsonTypeInfo<HashTask> HashTask
    {
        get => _hashTask ??= Create_HashTask(Options, makeReadOnly: true);
    }

    private JsonTypeInfo<HashTask> Create_HashTask(JsonSerializerOptions options, bool makeReadOnly)
    {
        JsonTypeInfo<HashTask>? jsonTypeInfo = null;
        JsonConverter? customConverter;
        if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(options, typeof(HashTask))) != null)
        {
            jsonTypeInfo = JsonMetadataServices.CreateValueInfo<HashTask>(options, customConverter);
        }
        else
        {
            JsonObjectInfoValues<HashTask> objectInfo = new JsonObjectInfoValues<HashTask>()
            {
                ObjectCreator = static () => new HashTask(),
                ObjectWithParameterizedConstructorCreator = null,
                PropertyMetadataInitializer = _ => HashTaskPropInit(options),
                ConstructorParameterMetadataInitializer = null,
                NumberHandling = default,
                SerializeHandler = null
            };

            jsonTypeInfo = JsonMetadataServices.CreateObjectInfo(options, objectInfo);
        }

        if (makeReadOnly)
        {
            jsonTypeInfo.MakeReadOnly();
        }

        return jsonTypeInfo;
    }

    private static JsonPropertyInfo[] HashTaskPropInit(JsonSerializerOptions options)
    {
        JsonPropertyInfo[] properties = new JsonPropertyInfo[8];

        JsonPropertyInfoValues<DateTime> info0 = new JsonPropertyInfoValues<DateTime>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).DateTime,
            Setter = static (obj, value) => ((HashTask)obj).DateTime = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "DateTime",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo0 = JsonMetadataServices.CreatePropertyInfo(options, info0);
        properties[0] = propertyInfo0;

        JsonPropertyInfoValues<HashTaskMode> info1 = new JsonPropertyInfoValues<HashTaskMode>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).Mode!,
            Setter = static (obj, value) => ((HashTask)obj).Mode = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "Mode",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo1 = JsonMetadataServices.CreatePropertyInfo(options, info1);
        properties[1] = propertyInfo1;

        JsonPropertyInfoValues<string> info2 = new JsonPropertyInfoValues<string>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).Content!,
            Setter = static (obj, value) => ((HashTask)obj).Content = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "Content",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo2 = JsonMetadataServices.CreatePropertyInfo(options, info2);
        properties[2] = propertyInfo2;

        JsonPropertyInfoValues<Encoding?> info3 = new JsonPropertyInfoValues<Encoding?>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = App.GetService<EncodingJsonConverter>(),
            Getter = static (obj) => ((HashTask)obj).Encoding!,
            Setter = static (obj, value) => ((HashTask)obj).Encoding = value!,
            IgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "Encoding",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo3 = JsonMetadataServices.CreatePropertyInfo(options, info3);
        properties[3] = propertyInfo3;

        JsonPropertyInfoValues<IList<Hash>> info4 = new JsonPropertyInfoValues<IList<Hash>>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).SelectedHashs!,
            Setter = static (obj, value) => ((HashTask)obj).SelectedHashs = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "SelectedHashs",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo4 = JsonMetadataServices.CreatePropertyInfo(options, info4);
        properties[4] = propertyInfo4;

        JsonPropertyInfoValues<TimeSpan> info5 = new JsonPropertyInfoValues<TimeSpan>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).Elapsed!,
            Setter = static (obj, value) => ((HashTask)obj).Elapsed = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "Elapsed",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo5 = JsonMetadataServices.CreatePropertyInfo(options, info5);
        properties[5] = propertyInfo5;

        JsonPropertyInfoValues<HashTaskState> info6 = new JsonPropertyInfoValues<HashTaskState>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).State!,
            Setter = static (obj, value) => ((HashTask)obj).State = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "State",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo6 = JsonMetadataServices.CreatePropertyInfo(options, info6);
        properties[6] = propertyInfo6;

        JsonPropertyInfoValues<ObservableCollection<HashResult>> info7 = new JsonPropertyInfoValues<ObservableCollection<HashResult>>()
        {
            IsProperty = true,
            IsPublic = true,
            IsVirtual = false,
            DeclaringType = typeof(HashTask),
            Converter = null,
            Getter = static (obj) => ((HashTask)obj).Results!,
            Setter = static (obj, value) => ((HashTask)obj).Results = value!,
            IgnoreCondition = null,
            HasJsonInclude = false,
            IsExtensionData = false,
            NumberHandling = default,
            PropertyName = "Results",
            JsonPropertyName = null
        };

        JsonPropertyInfo propertyInfo7 = JsonMetadataServices.CreatePropertyInfo(options, info7);
        properties[7] = propertyInfo7;

        return properties;
    }
}
#endregion Models.HashTask

#region IEnumerable<Models.HashTask>
public sealed partial class JsonContext2
{
    private JsonTypeInfo<IEnumerable<HashTask>>? _iEnumerableHashTask;
    public JsonTypeInfo<IEnumerable<HashTask>> IEnumerableHashTask
    {
        get => _iEnumerableHashTask ??= Create_IEnumerableHashTask(Options, makeReadOnly: true);
    }

    private JsonTypeInfo<IEnumerable<HashTask>> Create_IEnumerableHashTask(JsonSerializerOptions options, bool makeReadOnly)
    {
        JsonTypeInfo<IEnumerable<HashTask>>? jsonTypeInfo = null;
        JsonConverter? customConverter;
        if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(options, typeof(IEnumerable<HashTask>))) != null)
        {
            jsonTypeInfo = JsonMetadataServices.CreateValueInfo<IEnumerable<HashTask>>(options, customConverter);
        }
        else
        {
            JsonCollectionInfoValues<IEnumerable<HashTask>> info = new JsonCollectionInfoValues<IEnumerable<HashTask>>()
            {
                ObjectCreator = null,
                NumberHandling = default,
                SerializeHandler = IEnumerableHashTaskSerializeHandler
            };

            jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<HashTask>, HashTask>(options, info);

        }

        if (makeReadOnly)
        {
            jsonTypeInfo.MakeReadOnly();
        }

        return jsonTypeInfo;
    }

    // Intentionally not a static method because we create a delegate to it. Invoking delegates to instance
    // methods is almost as fast as virtual calls. Static methods need to go through a shuffle thunk.
    private void IEnumerableHashTaskSerializeHandler(Utf8JsonWriter writer, IEnumerable<HashTask>? value)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (HashTask element in value)
        {
            JsonSerializer.Serialize(writer, element!, Default.HashTask!);
        }

        writer.WriteEndArray();
    }
}
#endregion IEnumerable<Models.HashTask>

#region System.Text.Encoding
public sealed partial class JsonContext2
{
    private JsonTypeInfo<Encoding>? _encoding;
    public JsonTypeInfo<Encoding> Encoding
    {
        get => _encoding ??= Create_Encoding(Options, makeReadOnly: true);
    }

    private JsonTypeInfo<Encoding> Create_Encoding(JsonSerializerOptions options, bool makeReadOnly)
    {
        JsonTypeInfo<Encoding>? jsonTypeInfo = null;
        JsonConverter? customConverter;
        if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(options, typeof(Encoding))) != null)
        {
            jsonTypeInfo = JsonMetadataServices.CreateValueInfo<Encoding>(options, customConverter);
        }
        else
        {
            jsonTypeInfo = JsonMetadataServices.CreateValueInfo<Encoding>(options, App.GetService<EncodingJsonConverter>());
        }

        if (makeReadOnly)
        {
            jsonTypeInfo.MakeReadOnly();
        }

        return jsonTypeInfo;
    }
}
#endregion System.Text.Encoding
