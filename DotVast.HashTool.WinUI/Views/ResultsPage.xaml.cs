using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class ResultsPage : Page
{
    public ResultsViewModel ViewModel { get; }

    public ResultsPage()
    {
        ViewModel = App.GetService<ResultsViewModel>();
        Resources.AddExpression(() => ViewModel);
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is HashTask hashTask)
        {
            Loaded += ResultsPage_Loaded_OnHashTaskChanged;
        }
    }

    private void ResultsPage_Loaded_OnHashTaskChanged(object sender, RoutedEventArgs e)
    {
        ContentScrollViewer.ChangeView(null, 0, null);

        Loaded -= ResultsPage_Loaded_OnHashTaskChanged;
    }
}
