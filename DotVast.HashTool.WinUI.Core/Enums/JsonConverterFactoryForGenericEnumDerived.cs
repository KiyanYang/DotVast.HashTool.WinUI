using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Core.Enums;

/// <summary>
/// 用于 <see cref="GenericEnum{T}"/> 派生类的 JSON 转换器.
/// </summary>
/// <remarks>
/// 该转换器仅支持直接从 <see cref="GenericEnum{T}"/> 派生的类, 且其枚举必须采用公共静态只读字段.
/// <code>public static readonly TEnum EnumName = Enum;</code>
/// </remarks>
public sealed class JsonConverterFactoryForGenericEnumDerived : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.BaseType?.IsGenericType ?? false
        && typeToConvert.BaseType?.GetGenericTypeDefinition() == typeof(GenericEnum<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var enumType = typeToConvert;
        var keyType = typeToConvert.BaseType!.GetGenericArguments()[0];

        return (JsonConverter)Activator.CreateInstance(
            typeof(JsonConverterForGenericEnum<,>).MakeGenericType(new[] { enumType, keyType }),
            args: new[] { options })!;
    }
}

sealed file class JsonConverterForGenericEnum<TEnum, TKey> : JsonConverter<TEnum>
    where TEnum : GenericEnum<TKey>
{
    private readonly TEnum[] _enums;
    private readonly Type _keyType;
    private readonly JsonConverter<TKey> _keyConverter;

    public JsonConverterForGenericEnum(JsonSerializerOptions options)
    {
        _enums = GenericEnum.GetFieldValues<TEnum>();
        _keyType = typeof(TKey);
        _keyConverter = (JsonConverter<TKey>)options.GetConverter(_keyType);
    }

    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var key = _keyConverter.Read(ref reader, _keyType, options);
        return _enums.FirstOrDefault(i => EqualityComparer<TKey>.Default.Equals(i._key, key));
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        _keyConverter.Write(writer, value._key, options);
    }
}
