using DotVast.HashTool.WinUI.Contracts.Services;

using Windows.Globalization;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class LanguageSelectorService : ILanguageSelectorService
{
    public AppLanguage Language { get; set; } = AppLanguage.ZhHans;

    public AppLanguage[] Languages { get; set; } = Array.Empty<AppLanguage>();

    public async Task InitializeAsync()
    {
        Languages = new AppLanguage[] { AppLanguage.ZhHans, AppLanguage.EnUS };
        Language = Languages.Where(x => x.Tag == ApplicationLanguages.PrimaryLanguageOverride)
                            .FirstOrDefault() ?? AppLanguage.ZhHans;
        await Task.CompletedTask;
    }

    public async Task SetAppLanguageAsync(AppLanguage language)
    {
        ApplicationLanguages.PrimaryLanguageOverride = language.Tag;
        await Task.CompletedTask;
    }
}
