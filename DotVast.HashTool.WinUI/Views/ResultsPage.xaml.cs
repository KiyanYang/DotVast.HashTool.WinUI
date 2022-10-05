using DotVast.HashTool.WinUI.Enums;
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
        base.OnNavigatedTo(e);
        if (e.Parameter is HashTask hashTask)
        {
            ViewModel.HashTask = hashTask;
            Loaded += ResultsPage_Loaded_OnHashTaskChanged;
        }
        else if (e.Parameter != null)
        {
            throw new ArgumentException("导航事件参数传递的变量类型不是 HashTask");
        }
    }

    private void ResultsPage_Loaded_OnHashTaskChanged(object sender, RoutedEventArgs e)
    {
        ContentScrollViewer.ChangeView(null, 0, null);

        #region {x:Bind} function to nullable, property doesn't update on null 缓解措施
        // https://github.com/microsoft/microsoft-ui-xaml/issues/1904
        // https://github.com/microsoft/microsoft-ui-xaml/issues/2166
        // 修复后用法 {x:Bind helpers:XamlFunctionHelper.VisibleIfNotNull(ViewModel.HashResults)}
        HashResultsTitle.Visibility = ViewModel.HashTask?.State != HashTaskState.Aborted
            ? Visibility.Visible : Visibility.Collapsed;
        #endregion

        Loaded -= ResultsPage_Loaded_OnHashTaskChanged;
    }
}
