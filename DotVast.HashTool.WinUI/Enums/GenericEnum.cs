namespace DotVast.HashTool.WinUI.Enums;

public class GenericEnum<T>
{
    protected readonly T _value;

    protected GenericEnum(T value)
    {
        _value = value;
    }

    public override string ToString() => _value?.ToString() ?? string.Empty;
}
