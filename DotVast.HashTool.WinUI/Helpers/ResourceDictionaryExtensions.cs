using System.Linq.Expressions;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class ResourceDictionaryExtensions
{
    public static bool AddExpression<T>(this ResourceDictionary resources, Expression<Func<T>> expression)
    {
        if (expression.Body is MemberExpression member)
        {
            resources.Add(member.Member.Name, expression.Compile()());
            return true;
        }
        return false;
    }
}
