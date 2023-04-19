// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Helpers;

public static class FrameExtensions
{
    public static IViewModel? GetPageViewModel(this Frame frame) =>
        (frame.Content as IView)?.ViewModel;
}
