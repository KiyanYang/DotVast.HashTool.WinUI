// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Behaviors;
using DotVast.HashTool.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views;

public sealed partial class HashSettingsPage : Page, IView
{
    public HashSettingsViewModel ViewModel { get; }

    IViewModel IView.ViewModel => ViewModel;

    public HashSettingsPage()
    {
        ViewModel = App.GetService<HashSettingsViewModel>();
        Resources.AddByExpression(() => ViewModel.HashFormats);
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderContext(this, Localization.HashSettingsPage_Title);
    }
}
