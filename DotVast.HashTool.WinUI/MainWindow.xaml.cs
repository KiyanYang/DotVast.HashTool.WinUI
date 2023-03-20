using DotVast.HashTool.WinUI.Contracts.Services;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;

using WinUIEx;

namespace DotVast.HashTool.WinUI;

public sealed partial class MainWindow : WindowEx
{
    private readonly ILogger<MainWindow> _logger = App.GetLogger<MainWindow>();

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

        Closed += (_, _) =>
        {
            foreach(var hashTask in App.GetService<IHashTaskService>().HashTasks)
            {
                hashTask.Cancel();
            }
            _logger.WindowClosed(typeof(MainWindow).FullName);
        };

        AppWindow.Destroying += (_, _) =>
        {
            _logger.AppWindowDestroying(typeof(MainWindow).FullName);
        };
    }

    public void TryEnqueue(DispatcherQueueHandler handler)
    {
        DispatcherQueue?.TryEnqueue(handler);
    }
}
