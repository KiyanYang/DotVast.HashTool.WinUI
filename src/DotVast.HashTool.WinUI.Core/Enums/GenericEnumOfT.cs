namespace DotVast.HashTool.WinUI.Core.Enums;

/// <summary>
/// 泛型枚举.
/// </summary>
/// <typeparam name="T">与泛型枚举绑定的键类型.</typeparam>
public class GenericEnum<T> : GenericEnum
{
    protected internal readonly T _key;

    /// <summary>
    /// 构造函数.
    /// </summary>
    /// <param name="key">标识枚举的键.</param>
    protected GenericEnum(T key)
    {
        _key = key;
    }

    public override string? ToString() =>
        _key?.ToString();
}
