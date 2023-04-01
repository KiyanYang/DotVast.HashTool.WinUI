using DotVast.HashTool.WinUI.Core.Enums;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Enums;

public sealed class AppLanguage : GenericEnum<string>
{
    public static readonly AppLanguage ZhHans = new("zh-Hans");
    public static readonly AppLanguage EnUS = new("en-US");

    private readonly Language _language;

    public string Tag => _language.LanguageTag;

    public string DisplayName => _language.DisplayName;

    public string NativeName => _language.NativeName;

    private AppLanguage(string languageTag) : base(languageTag)
    {
        _language = new(languageTag);
    }
}
