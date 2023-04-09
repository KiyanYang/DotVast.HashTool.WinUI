using DotVast.HashTool.WinUI.Strings.Base;
using DotVast.Toolkit.StringResource;

namespace DotVast.HashTool.WinUI.Strings;

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
internal sealed partial class LocalizationPopup : BaseLocalization
{
    internal const string SubtreeId = "Popups";
}

[StringResource("../zh-Hans/" + SubtreeId + ".resw", "public static string {0} => __{0} ??= GetLocalized(\"" + SubtreeId + "\", \"{0}\");\nprivate static string? __{0};")]
internal sealed partial class LocalizationEnum : BaseLocalization
{
    internal const string SubtreeId = "Enums";
}
