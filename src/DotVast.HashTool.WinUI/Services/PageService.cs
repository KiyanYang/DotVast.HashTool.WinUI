// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.ViewModels;
using DotVast.HashTool.WinUI.Views;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Services;

public sealed class PageService : IPageService
{
    private readonly Dictionary<PageKey, Type> _pages = new();

    public PageService()
    {
        Configure<SettingsViewModel, SettingsPage>(PageKey.SettingsPage);
        Configure<HomeViewModel, HomePage>(PageKey.HomePage);
        Configure<TasksViewModel, TasksPage>(PageKey.TasksPage);
        Configure<ResultsViewModel, ResultsPage>(PageKey.ResultsPage);
        Configure<LicensesViewModel, LicensesPage>(PageKey.LicensesPage);
        Configure<HashSettingsViewModel, HashSettingsPage>(PageKey.HashSettingsPage);
    }

    public Type GetPageType(PageKey key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>(PageKey key)
        where VM : IViewModel
        where V : Page, IView
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
