// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class ResultsPage : Page, IView
{
    public ResultsViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public ResultsPage()
    {
        ViewModel = App.GetService<ResultsViewModel>();
        Resources.AddByExpression(() => ViewModel);
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
