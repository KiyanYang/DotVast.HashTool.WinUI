using System.Linq.Expressions;

using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceManager s_resourceManager;
    private static readonly ResourceContext s_resourceContext;
    private static readonly ResourceMap s_resourceMap;

    static ResourceExtensions()
    {
        s_resourceManager = new();
        s_resourceContext = s_resourceManager.CreateResourceContext();
        s_resourceContext.QualifierValues["Language"] = ApplicationLanguages.PrimaryLanguageOverride;
        s_resourceMap = s_resourceManager.MainResourceMap;
    }

    public static string GetLocalized(this string resourceKey, string subtree = "Resources") =>
        s_resourceMap.GetSubtree(subtree).GetValue(resourceKey, s_resourceContext).ValueAsString;

    public static bool TryAdd<T>(this ResourceDictionary resources, Expression<Func<T>> expression)
    {
        if (expression.Body is MemberExpression member)
        {
            resources.Add(member.Member.Name, expression.Compile()());
            return true;
        }
        return false;
    }
}
