// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

using DotVast.HashTool.WinUI.Core.Helpers;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Services;

internal sealed class TextExportResolver : IExportResolver
{
    public bool CanResolve(ExportKind exportKind, object obj)
    {
        return exportKind == (ExportKind.Text | ExportKind.HashTask)
            && obj is IEnumerable<HashTask> hashTasks
            && hashTasks.Any();
    }

    public Task ExportAsync(string filePath, ExportKind exportKind, object obj)
    {
        if (!CanResolve(exportKind, obj))
        {
            throw new InvalidOperationException();
        }

        var hashTasks = (IEnumerable<HashTask>)obj;

        using var streamWriter = new StreamWriter(filePath);
        var indentedTextWriter = new IndentedTextWriter(streamWriter, "    ");

        WriteHashTask(indentedTextWriter, hashTasks.First());
        foreach (var hashTask in hashTasks.Skip(1))
        {
            indentedTextWriter.WriteLine();
            indentedTextWriter.WriteLine("----------------------------------------------------------------");
            indentedTextWriter.WriteLine();
            WriteHashTask(indentedTextWriter, hashTask);
        }

        return Task.CompletedTask;
    }

    private void WriteHashTask(IndentedTextWriter writer, HashTask hashTask)
    {
        writer.WritePropertyAndValue(hashTask.Mode.ToDisplay(), hashTask.Content);
        writer.WritePropertyAndValue(LocalizationCommon.Elapsed, hashTask.Elapsed.ToString());
        writer.WritePropertyAndValue(LocalizationCommon.State, hashTask.State.ToDisplay());
        writer.WritePropertyAndJoinValue(LocalizationCommon.Algorithms, ", ", hashTask.SelectedHashKinds.Select(h => h.ToName()));

        var results = hashTask.Results;
        if (results is null || !results.Any())
        {
            writer.WritePropertyAndValue(LocalizationCommon.Results, LocalizationCommon.Empty);
            return;
        }
        writer.WritePropertyAndValue(LocalizationCommon.Results, string.Empty);
        foreach (var result in results)
        {
            writer.Write("[");
            writer.Write(result.Path);
            writer.WriteLine("]");

            var data = result.Data;

            // 正常情况下有 HashResult 就至少有一个 HashResultItem, 因此一般不会进入该判断内
            if (data is null || !data.Any())
            {
                writer.WriteLine(LocalizationCommon.Empty);
                continue;
            }

            writer.Indent++;
            foreach (var resultItem in data)
            {
                writer.WritePropertyAndValue(resultItem.Name, resultItem.Value);
            }
            writer.Indent--;
        }
    }
}
