// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

namespace DotVast.HashTool.WinUI.Core.Helpers;

public static class IndentedTextWriterExtensions
{
    private const string ColonSeparator = ": ";

    public static void WriteJoin<T>(this IndentedTextWriter writer, ReadOnlySpan<char> separator, IEnumerable<T> values)
    {
        if (!values.Any())
        {
            return;
        }
        writer.Write(values.First());
        foreach (var item in values.Skip(1))
        {
            writer.Write(separator);
            writer.Write(item);
        }
    }

    public static void WritePropertyAndValue(this IndentedTextWriter writer, string property, string value)
    {
        writer.Write(property);
        writer.Write(ColonSeparator);
        writer.WriteLine(value);
    }

    public static void WritePropertyAndJoinValue<T>(this IndentedTextWriter writer, string property, ReadOnlySpan<char> separator, IEnumerable<T> values)
    {
        writer.Write(property);
        writer.Write(ColonSeparator);
        writer.WriteJoin(separator, values);
        writer.WriteLine();
    }
}
