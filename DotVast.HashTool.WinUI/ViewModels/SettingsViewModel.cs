using System.Reflection;

using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _versionDescription;

    #region Language selector

    private readonly ILanguageSelectorService _languageSelectorService;

    public AppLanguage[] AppLanguages
    {
        get;
    }

    [ObservableProperty]
    private AppLanguage _appLanguage;

    async partial void OnAppLanguageChanged(AppLanguage value)
    {
        await _languageSelectorService.SetAppLanguageAsync(value);
    }

    #endregion Language selector

    #region Theme selector

    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [RelayCommand]
    private async Task SwitchTheme(ElementTheme param)
    {
        if (ElementTheme != param)
        {
            ElementTheme = param;
            await _themeSelectorService.SetThemeAsync(param);
        }
    }

    #endregion Theme selector

    public SettingsViewModel(IThemeSelectorService themeSelectorService, ILanguageSelectorService languageSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _languageSelectorService = languageSelectorService;
        _appLanguage = _languageSelectorService.Language;
        AppLanguages = _languageSelectorService.Languages;
        _versionDescription = GetVersionDescription();
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
