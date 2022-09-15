namespace DotVast.HashTool.WinUI.Helpers;

public class GenericEnum<T>
{
    protected readonly T _value;

    protected GenericEnum(T value)
    {
        _value = value;
    }

    public override string ToString() => _value?.ToString() ?? string.Empty;
}
