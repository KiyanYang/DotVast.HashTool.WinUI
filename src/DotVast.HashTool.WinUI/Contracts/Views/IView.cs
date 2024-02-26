// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Contracts.Views;

public interface IView
{
    IViewModel ViewModel { get; }
}

public interface IView<TViewModel> : IView where TViewModel : IViewModel
{
    IViewModel IView.ViewModel => ViewModel;

    new TViewModel ViewModel { get; }
}
