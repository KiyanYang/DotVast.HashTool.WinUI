using System.Reflection;

namespace DotVast.HashTool.WinUI.Core.Enums;

public class GenericEnum
{
    public static TEnum[] GetFieldValues<TEnum>()
        where TEnum : GenericEnum
    {
        return typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public)
                            .Select(i => (TEnum)i.GetValue(null)!)
                            .ToArray();
    }
}
