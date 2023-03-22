using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class TasksPage : Page, IView
{
    public AuxiliaryObject Auxiliary { get; }
    public TasksViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public TasksPage()
    {
        Auxiliary = new();
        ViewModel = App.GetService<TasksViewModel>();
        Resources.AddExpression(() => Auxiliary);
        Resources.AddExpression(() => ViewModel.SaveCommand);
        Resources.AddExpression(() => ViewModel.ShowResultCommand);
        Resources.AddExpression(() => Localization.Command_Save_Tip);
        InitializeComponent();
    }

    public sealed partial class AuxiliaryObject : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelectEnabled;

        public AuxiliaryObject()
        {
            _isSelectEnabled = false;
        }
    }
}
