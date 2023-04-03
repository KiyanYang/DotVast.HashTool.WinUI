using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IPageService
{
    Type GetPageType(PageKey key);
}
