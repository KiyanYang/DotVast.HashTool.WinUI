using System.Linq.Expressions;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Helpers;

public static class ResourceDictionaryExtensions
{
    /// <summary>
    /// Adds a resource to the <see cref="FrameworkElement.Resources"/> by using a lambda expression to specify
    /// the resource key and value. For example: <c>Resources.AddByExpression(() => Foo.Bar.Baz);</c> means adding
    /// Baz to Resources, which is equivalent to <c>Resources.Add(nameof(Foo.Bar.Baz), Foo.Bar.Baz);</c>.
    /// </summary>
    /// <typeparam name="T">The type of the value to the resources.</typeparam>
    /// <param name="resources">The <see cref="FrameworkElement.Resources"/> to add the resource.</param>
    /// <param name="expression">A lambda expression that specifies the resource key and value.</param>
    /// <exception cref="ArgumentException">The expression.Body must be MemberExpression.</exception>
    public static void AddByExpression<T>(this ResourceDictionary resources, Expression<Func<T>> expression)
    {
        if (expression.Body is not MemberExpression member)
        {
            throw new ArgumentException($"The expression.Body must be MemberExpression.");
        }

        resources.Add(member.Member.Name, expression.Compile()());
        return;
    }
}
