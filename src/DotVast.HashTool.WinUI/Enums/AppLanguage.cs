// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

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
    [field: AllowNull]
    private static AppLanguage[] _Values => field ??= Enum.GetValues<AppLanguage>();

    [field: AllowNull]
    private static Language _EnUS => field ??= new("en-US");

    [field: AllowNull]
    private static Language _ZhHans => field ??= new("zh-Hans");

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
