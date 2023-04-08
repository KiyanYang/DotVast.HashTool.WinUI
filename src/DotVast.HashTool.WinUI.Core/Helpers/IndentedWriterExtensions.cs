namespace DotVast.HashTool.WinUI.Core.Helpers;

public static class IndentedWriterExtensions
{
    private const string ColonSeparator = ": ";

    public static void WritePropertyAndValue(this IndentedWriter writer, string property, string value)
    {
        writer.Write(property);
        writer.Write(ColonSeparator);
        writer.WriteLine(value);
    }

    public static void WritePropertyAndJoinValue<T>(this IndentedWriter writer, string property, ReadOnlySpan<char> separator, IEnumerable<T> values)
    {
        writer.Write(property);
        writer.Write(ColonSeparator);
        writer.WriteJoin(separator, values);
        writer.WriteLine();
    }
}
