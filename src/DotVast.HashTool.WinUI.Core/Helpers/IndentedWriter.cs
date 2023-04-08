namespace DotVast.HashTool.WinUI.Core.Helpers;

public struct IndentedWriter
{
    private readonly TextWriter _writer;
    private readonly string _tabString;
    private int _indentLevel = 0;
    private bool _tabsPending = true;

    public IndentedWriter(TextWriter writer, string tabString)
    {
        _writer = writer;
        _tabString = tabString;
    }

    public int Indent
    {
        get => _indentLevel;
        set => _indentLevel = Math.Max(value, 0);
    }

    public void Write(string value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    public void WriteLine()
    {
        WriteLine(string.Empty);
    }

    public void WriteLine(string value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    public void WriteJoin<T>(ReadOnlySpan<char> separator, IEnumerable<T> values)
    {
        if (!values.Any())
        {
            return;
        }
        _writer.Write(values.First());
        foreach (var item in values.Skip(1))
        {
            _writer.Write(separator);
            _writer.Write(item);
        }
    }

    private void OutputTabs()
    {
        if (_tabsPending)
        {
            for (var i = 0; i < _indentLevel; i++)
            {
                _writer.Write(_tabString);
            }
            _tabsPending = false;
        }
    }
}
