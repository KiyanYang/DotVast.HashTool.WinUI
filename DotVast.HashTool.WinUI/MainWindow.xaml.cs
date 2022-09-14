using DotVast.HashTool.WinUI.Helpers;

namespace DotVast.HashTool.WinUI;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
