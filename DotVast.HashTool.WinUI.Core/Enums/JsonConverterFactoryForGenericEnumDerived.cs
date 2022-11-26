using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Core.Enums;

/// <summary>
/// 用于 <see cref="GenericEnum{T}"/> 派生类的 JSON 转换器.
/// </summary>
/// <remarks>
/// <see cref="GenericEnum{T}"/> 派生类中的枚举必须采用公共静态只读字段, 才支持该转换器.
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
        var valueType = typeToConvert.BaseType!.GetGenericArguments()[0];

        return (JsonConverter)Activator.CreateInstance(
            typeof(JsonConverterForGenericEnum<,>).MakeGenericType(new[] { enumType, valueType }),
            args: new[] { options })!;
    }
}

sealed file class JsonConverterForGenericEnum<TEnum, TValue> : JsonConverter<TEnum>
    where TEnum : GenericEnum<TValue>
{
    private readonly Type _enumType;
    private readonly Type _valueType;
    private readonly JsonConverter<TValue> _valueConverter;
    private readonly IList<TEnum> _enums;

    [SuppressMessage("Style", "IDE1006:命名样式")]
    private readonly Func<TEnum, TValue> _GetValue;

    public JsonConverterForGenericEnum(JsonSerializerOptions options)
    {
        _enumType = typeof(TEnum);
        _valueType = typeof(TValue);
        _valueConverter = (JsonConverter<TValue>)options.GetConverter(_valueType);
        _enums = GenericEnum.GetFieldValues<TEnum>();

        var valueInfo = _enumType.BaseType!.GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var parameterExpression = Expression.Parameter(_enumType);
        var valueBody = Expression.Field(parameterExpression, valueInfo);
        _GetValue = Expression.Lambda<Func<TEnum, TValue>>(valueBody, parameterExpression).Compile();
    }

    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = _valueConverter.Read(ref reader, _valueType, options);
        return _enums.FirstOrDefault(i => EqualityComparer<TValue>.Default.Equals(_GetValue(i), value));
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        _valueConverter.Write(writer, _GetValue(value), options);
    }
}
