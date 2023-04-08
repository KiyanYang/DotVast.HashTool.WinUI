using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal class JsonExportResolver : IExportResolver
{
    public bool CanResolve(ExportKind exportKind, object obj)
    {
        return exportKind == (ExportKind.Json | ExportKind.HashTask)
            && obj is IEnumerable<HashTask>;
    }

    public async Task ExportAsync(string filePath, ExportKind exportKind, object obj)
    {
        if (!CanResolve(exportKind, obj))
        {
            throw new InvalidOperationException();
        }

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
        };

        var hashTasks = (IEnumerable<HashTask>)obj;
        if (hashTasks.Take(2).Count() == 1)
        {
            var contents = JsonSerializer.Serialize(hashTasks.First(), new JsonContext(options).HashTask);
            await File.WriteAllTextAsync(filePath, contents);
        }
        else
        {
            var contents = JsonSerializer.Serialize(hashTasks, new JsonContext(options).IEnumerableHashTask);
            await File.WriteAllTextAsync(filePath, contents);
        }
    }
}
