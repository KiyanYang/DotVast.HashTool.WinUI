using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Enums;

public sealed class AppLanguage : GenericEnum<Language>
{
    public static readonly AppLanguage ZhHans = new("zh-Hans");
    public static readonly AppLanguage EnUS = new("en-US");

    public string Tag => _value.LanguageTag;

    public string DisplayName => _value.DisplayName;

    public string NativeName => _value.NativeName;

    private AppLanguage(string languageTag) : base(new(languageTag)) { }
}
