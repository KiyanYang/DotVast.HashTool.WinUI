using CharacterEncoding = System.Text.Encoding;

namespace DotVast.HashTool.WinUI.Models;

public readonly record struct TextEncoding(string Name, Lazy<CharacterEncoding> Encoding)
{
    public static readonly TextEncoding UTF8 = new(CharacterEncoding.UTF8.WebName.ToUpper(), new(() => CharacterEncoding.UTF8));

    public bool Equals(TextEncoding other) => Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => HashCode.Combine(Name, Encoding);
}
