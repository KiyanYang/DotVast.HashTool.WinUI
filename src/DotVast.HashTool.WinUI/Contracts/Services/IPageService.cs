// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IPageService
{
    Type GetPageType(PageKey key);
}
