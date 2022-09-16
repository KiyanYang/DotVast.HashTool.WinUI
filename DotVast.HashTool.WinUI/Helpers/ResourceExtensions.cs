using Microsoft.Windows.ApplicationModel.Resources;

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
        s_resourceContext.QualifierValues["Language"] = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        s_resourceMap = s_resourceManager.MainResourceMap.GetSubtree("Resources");
    }

    public static string GetLocalized(this string resourceKey) =>
        s_resourceMap.GetValue(resourceKey, s_resourceContext).ValueAsString;
}
