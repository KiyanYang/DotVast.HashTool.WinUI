// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class TasksPage : Page, IView<TasksViewModel>
{
    public AuxiliaryObject Auxiliary { get; }
    public TasksViewModel ViewModel { get; }

    public TasksPage()
    {
        Auxiliary = new();
        ViewModel = App.GetService<TasksViewModel>();
        Resources.AddByExpression(() => Auxiliary);
        Resources.AddByExpression(() => ViewModel.SaveCommand);
        Resources.AddByExpression(() => ViewModel.ShowResultCommand);
        Resources.AddByExpression(() => Localization.Command_Save_Tip);
        InitializeComponent();
    }

    public sealed partial class AuxiliaryObject : ObservableObject
    {
        [ObservableProperty]
        public partial bool IsSelectEnabled { get; set; } = false;
    }
}
