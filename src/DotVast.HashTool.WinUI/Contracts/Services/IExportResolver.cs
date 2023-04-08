using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IExportResolver
{
    /// <summary>
    /// 检查支持导出处理.
    /// </summary>
    /// <param name="exportKind">导出类型.</param>
    /// <param name="obj">导出内容.</param>
    /// <returns>若支持导出返回 <see langword="true"/>, 否则为 <see langword="false"/>.</returns>
    bool CanResolve(ExportKind exportKind, object obj);

    /// <summary>
    /// 导出内容.
    /// </summary>
    /// <param name="filePath">导出路径.</param>
    /// <param name="exportKind">导出类型.</param>
    /// <param name="obj">导出内容.</param>
    /// <exception cref="InvalidOperationException">当不支持该类型或内容的导出时, 抛出该异常.</exception>
    Task ExportAsync(string filePath, ExportKind exportKind, object obj);
}
