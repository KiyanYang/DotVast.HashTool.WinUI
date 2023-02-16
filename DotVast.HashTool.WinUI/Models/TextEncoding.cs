using System.Text;

namespace DotVast.HashTool.WinUI.Models;

public readonly record struct TextEncoding(string Name, Lazy<Encoding> Encoding)
{
    public bool Equals(TextEncoding other) => Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => HashCode.Combine(Name, Encoding);
}
