// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.Windows.ApplicationModel.Resources;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Strings.Base;

internal abstract class BaseLocalization
{
    private const string LanguageQualifier = "Language";

    private static readonly ResourceManager s_resourceManager;
    private static readonly ResourceContext s_resourceContext;
    private static readonly ResourceMap s_resourceMap;
    private static readonly string s_defaultLanguage;

    private static ResourceContext? s_tmpResourceContext;

    static BaseLocalization()
    {
        s_resourceManager = new ResourceManager();
        s_resourceContext = s_resourceManager.CreateResourceContext();
        s_defaultLanguage = s_resourceContext.QualifierValues[LanguageQualifier];
        SetLanguageQualifierValue(s_resourceContext, ApplicationLanguages.PrimaryLanguageOverride);
        s_resourceMap = s_resourceManager.MainResourceMap;
    }

    internal static string GetLocalized(string subtreeId, string resourceKey) =>
        s_resourceMap.GetSubtree(subtreeId).GetValue(resourceKey, s_resourceContext).ValueAsString;

    internal static string GetLocalized(string subtreeId, string resourceKey, string language)
    {
        s_tmpResourceContext ??= s_resourceManager.CreateResourceContext();
        SetLanguageQualifierValue(s_tmpResourceContext, language);
        return s_resourceMap.GetSubtree(subtreeId).GetValue(resourceKey, s_tmpResourceContext).ValueAsString;
    }

    internal static void SetLanguageQualifierValue(ResourceContext resourceContext, string language)
    {
        resourceContext.QualifierValues[LanguageQualifier] = string.IsNullOrEmpty(language) ? s_defaultLanguage : language;
    }
}
