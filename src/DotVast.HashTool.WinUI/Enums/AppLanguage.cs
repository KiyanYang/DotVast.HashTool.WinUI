using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Enums;

public enum AppLanguage
{
    // System as the first
    System,

    // Others are sorted alphabetically
    EnUS,
    ZhHans,
}

public static class AppLanguageExtensions
{
    private static AppLanguage[]? s_appLanguages;
    private static AppLanguage[] _Values => s_appLanguages ??= Enum.GetValues<AppLanguage>();

    private static Language? s_enUS;
    private static Language? s_zhHans;

    private static Language _EnUS => s_enUS ??= new("en-US");
    private static Language _ZhHans => s_zhHans ??= new("zh-Hans");

    public static AppLanguage ToAppLanguage(this string languageTag)
    {
        return _Values.First(v => v.ToTag().Equals(languageTag, StringComparison.OrdinalIgnoreCase));
    }

    public static string ToTag(this AppLanguage appLanguage)
    {
        return appLanguage switch
        {
            AppLanguage.System => string.Empty,
            AppLanguage.EnUS => _EnUS.LanguageTag,
            AppLanguage.ZhHans => _ZhHans.LanguageTag,
            _ => throw new ArgumentOutOfRangeException(nameof(appLanguage))
        };
    }

    public static string ToNativeName(this AppLanguage appLanguage)
    {
        return appLanguage switch
        {
            AppLanguage.System => LocalizationEnum.AppLanguage_System_NativeName,
            AppLanguage.EnUS => _EnUS.NativeName,
            AppLanguage.ZhHans => _ZhHans.NativeName,
            _ => throw new ArgumentOutOfRangeException(nameof(appLanguage))
        };
    }
}
