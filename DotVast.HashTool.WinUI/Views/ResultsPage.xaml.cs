using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class ResultsPage : Page
{
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
            #region {x:Bind} function to nullable, property doesn't update on null 缓解措施
            // https://github.com/microsoft/microsoft-ui-xaml/issues/1904
            // https://github.com/microsoft/microsoft-ui-xaml/issues/2166
            // 修复后用法 {x:Bind helpers:XamlFunctionHelper.VisibleIfNotNull(ViewModel.HashResults)}
            HashResultsTitle.Visibility = hashTask.State != HashTaskState.Aborted
                ? Visibility.Visible : Visibility.Collapsed;
            #endregion
        }
        else if (e.Parameter != null)
        {
            throw new ArgumentException("导航事件参数传递的变量类型不是 HashTask");
        }
    }
}
