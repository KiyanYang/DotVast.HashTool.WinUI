using DotVast.Toolkit.StringResource;

using Microsoft.Windows.ApplicationModel.Resources;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Strings;

internal abstract class BaseLocalization
{
    private const string LanguageQualifier = "Language";

    private static readonly ResourceManager s_resourceManager;
    private static readonly ResourceContext s_resourceContext;
    private static readonly ResourceMap s_resourceMap;

    private static ResourceContext? s_tmpResourceContext;

    static BaseLocalization()
    {
        s_resourceManager = new ResourceManager();
        s_resourceContext = s_resourceManager.CreateResourceContext();
        s_resourceContext.QualifierValues[LanguageQualifier] = ApplicationLanguages.PrimaryLanguageOverride;
        s_resourceMap = s_resourceManager.MainResourceMap;
    }

    internal static string GetLocalized(string subtreeId, string resourceKey) =>
        s_resourceMap.GetSubtree(subtreeId).GetValue(resourceKey, s_resourceContext).ValueAsString;

    internal static string GetLocalized(string subtreeId, string resourceKey, string language)
    {
        s_tmpResourceContext ??= s_resourceManager.CreateResourceContext();
        s_tmpResourceContext.QualifierValues[LanguageQualifier] = language;
        return s_resourceMap.GetSubtree(subtreeId).GetValue(resourceKey, s_tmpResourceContext).ValueAsString;
    }
}

[StringResource("../zh-Hans/" + SubtreeId + ".resw", "public static string {0} => __{0} ??= GetLocalized(\"" + SubtreeId + "\", \"{0}\");\nprivate static string? __{0};")]
internal sealed partial class Localization : BaseLocalization
{
    internal const string SubtreeId = "Resources";
}

[StringResource("../zh-Hans/" + SubtreeId + ".resw", "public static string {0} => __{0} ??= GetLocalized(\"" + SubtreeId + "\", \"{0}\");\nprivate static string? __{0};")]
internal sealed partial class LocalizationCommon : BaseLocalization
{
    internal const string SubtreeId = "Common";
}

[StringResource("../zh-Hans/" + SubtreeId + ".resw", "public static string {0} => __{0} ??= GetLocalized(\"" + SubtreeId + "\", \"{0}\");\nprivate static string? __{0};")]
internal sealed partial class LocalizationDialog : BaseLocalization
{
    internal const string SubtreeId = "Dialogs";
}

[StringResource("../zh-Hans/" + SubtreeId + ".resw", "public static string {0} => __{0} ??= GetLocalized(\"" + SubtreeId + "\", \"{0}\");\nprivate static string? __{0};")]
internal sealed partial class LocalizationEnum : BaseLocalization
{
    internal const string SubtreeId = "Enums";
}
