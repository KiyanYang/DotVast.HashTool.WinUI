using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface ILanguageSelectorService
{
    AppLanguage Language
    {
        get;
    }

    AppLanguage[] Languages
    {
        get;
    }

    Task InitializeAsync();

    Task SetAppLanguageAsync(AppLanguage language);
}

public class AppLanguage : GenericEnum<Language>
{
    public static readonly AppLanguage ZhHans = new("zh-Hans");
    public static readonly AppLanguage EnUS = new("en-US");

    public string Tag => _value.LanguageTag;

    public string DisplayName => _value.DisplayName;

    public string NativeName => _value.NativeName;

    private AppLanguage(string languageTag) : base(new(languageTag)) { }
}
