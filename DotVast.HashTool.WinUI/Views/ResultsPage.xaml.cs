using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class ResultsPage : Page
{
    // TODO: Results_Detail_Encoding 隐藏时依旧会保留 StackLayout 间距.
    public ResultsViewModel ViewModel
    {
        get;
    }

    public ResultsPage()
    {
        ViewModel = App.GetService<ResultsViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is HashTask hashTask)
        {
            ViewModel.HashTask = hashTask;
        }
        else if (e.Parameter != null)
        {
            throw new ArgumentException("导航事件参数传递的变量类型不是 HashTask");
        }
    }
}
