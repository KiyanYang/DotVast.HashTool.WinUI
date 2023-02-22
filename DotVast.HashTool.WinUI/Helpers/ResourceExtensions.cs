using Microsoft.Windows.ApplicationModel.Resources;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceContext s_resourceContext;
    private static readonly ResourceMap s_resourceMap;

    static ResourceExtensions()
    {
        var resourceManager = new ResourceManager();
        s_resourceContext = resourceManager.CreateResourceContext();
        s_resourceContext.QualifierValues["Language"] = ApplicationLanguages.PrimaryLanguageOverride;
        s_resourceMap = resourceManager.MainResourceMap;
    }

    public static string GetLocalized(this string resourceKey, string subtree = "Resources") =>
        s_resourceMap.GetSubtree(subtree).GetValue(resourceKey, s_resourceContext).ValueAsString;
}
