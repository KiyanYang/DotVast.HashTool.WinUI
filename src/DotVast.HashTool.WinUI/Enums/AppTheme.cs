using DotVast.HashTool.WinUI.Core.Enums;

using Microsoft.UI.Xaml;

namespace DotVast.HashTool.WinUI.Enums;

public sealed class AppTheme : GenericEnum<string>
{
    public static readonly AppTheme Default = new(LocalizationEnum.AppTheme_Default, ElementTheme.Default);
    public static readonly AppTheme Light = new(LocalizationEnum.AppTheme_Light, ElementTheme.Light);
    public static readonly AppTheme Dark = new(LocalizationEnum.AppTheme_Dark, ElementTheme.Dark);

    public ElementTheme Theme { get; }

    private AppTheme(string name, ElementTheme theme) : base(name)
    {
        Theme = theme;
    }
}
