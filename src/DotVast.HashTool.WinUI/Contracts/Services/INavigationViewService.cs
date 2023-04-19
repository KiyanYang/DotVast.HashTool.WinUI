// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface INavigationViewService
{
    IList<object>? MenuItems { get; }

    object? SettingsItem { get; }

    void Initialize(NavigationView navigationView);

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);
}
