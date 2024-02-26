// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class SettingsPage : Page, IView<SettingsViewModel>
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        Resources.AddByExpression(() => ViewModel.RestartAppCommand);
        Resources.AddByExpression(() => Localization.Command_RestartApp);
        InitializeComponent();
    }
}
