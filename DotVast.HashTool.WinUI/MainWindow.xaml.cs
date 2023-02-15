using WinUIEx;

namespace DotVast.HashTool.WinUI;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icon.ico"));
        Content = null;
#if DEBUG
        Title = Localization.AppDisplayNameDev;
#else
        Title = Localization.AppDisplayName;
#endif
    }
}
