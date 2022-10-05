using DotVast.HashTool.WinUI.Enums;

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
