namespace DotVast.HashTool.WinUI.Core.Enums;

/// <summary>
/// 泛型枚举.
/// </summary>
/// <typeparam name="T">与枚举绑定的值.</typeparam>
public class GenericEnum<T> : GenericEnum
{
    // Field Name BindingTo GetField's Parameter Name which in JsonConverterForGenericEnum<,>.ctor
    private readonly T _value;

    protected GenericEnum(T value)
    {
        _value = value;
    }

    public override string? ToString() =>
        _value?.ToString();
}
