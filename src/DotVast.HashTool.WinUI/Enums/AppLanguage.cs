using DotVast.HashTool.WinUI.Core.Enums;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Enums;

public sealed class AppLanguage : GenericEnum<string>
{
    // System as the first
    public static readonly AppLanguage System = new("", LocalizationEnum.AppLanguage_System_NativeName);

    // Others are sorted alphabetically
    public static readonly AppLanguage EnUS = new("en-US");
    public static readonly AppLanguage ZhHans = new("zh-Hans");

    public string Tag { get; }

    public string NativeName { get; }

    private AppLanguage(string languageTag) : base(languageTag)
    {
        var language = new Language(languageTag);
        Tag = languageTag;
        NativeName = language.NativeName;
    }

    private AppLanguage(string tag, string nativeName) : base(tag)
    {
        Tag = tag;
        NativeName = nativeName;
    }
}
