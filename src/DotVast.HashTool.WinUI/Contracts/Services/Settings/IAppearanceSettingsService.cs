// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IAppearanceSettingsService : IBaseObservableSettings
{
    /// <summary>
    /// 总是置顶.
    /// </summary>
    bool IsAlwaysOnTop { get; set; }

    /// <summary>
    /// 用于显示哈希的字体系列名称.
    /// </summary>
    string HashFontFamilyName { get; set; }

    /// <summary>
    /// 主题.
    /// </summary>
    AppTheme Theme { get; set; }

    /// <summary>
    /// 语言.
    /// </summary>
    AppLanguage Language { get; set; }
}
