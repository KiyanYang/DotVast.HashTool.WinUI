// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Views;

namespace DotVast.HashTool.WinUI.Services;

public sealed class PageService : IPageService
{
    private readonly Lock _lock = new();
    private readonly Dictionary<PageKey, Type> _pages = new()
        {
            { PageKey.SettingsPage, typeof(SettingsPage)},
            { PageKey.HomePage, typeof(HomePage)},
            { PageKey.TasksPage, typeof(TasksPage)},
            { PageKey.ResultsPage, typeof(ResultsPage)},
            { PageKey.LicensesPage, typeof(LicensesPage)},
            { PageKey.HashSettingsPage, typeof(HashSettingsPage)},
        };

    public PageService() { }

    public Type GetPageType(PageKey key)
    {
        Type? pageType;
        lock (_lock)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }
}
