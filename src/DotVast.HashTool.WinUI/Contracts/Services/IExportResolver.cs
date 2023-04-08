using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IExportResolver
{
    bool CanResolve(ExportKind exportKind, object obj);

    Task ExportAsync(string filePath, ExportKind exportKind, object obj);
}
