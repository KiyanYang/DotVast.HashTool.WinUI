using System.Text;

namespace DotVast.HashTool.WinUI.Models;

public readonly record struct TextEncoding(string Name, Lazy<Encoding> Encoding)
{
    private static TextEncoding[]? s_all;

    static TextEncoding()
    {
        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        EncodingInfos = System.Text.Encoding.GetEncodings();
        UTF8 = new(System.Text.Encoding.UTF8.WebName.ToUpper(), new(() => System.Text.Encoding.UTF8));
    }

    internal static EncodingInfo[] EncodingInfos { get; }
    public static TextEncoding[] All => s_all ??= CreateAll();
    public static TextEncoding UTF8 { get; }

    private static TextEncoding[] CreateAll()
    {
        return EncodingInfos
          .Select(e => new TextEncoding(e.Name.ToUpper(), new(() => e.GetEncoding())))
          .OrderBy(t => t.Name)
          .ToArray();
    }

    public bool Equals(TextEncoding other) => Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => HashCode.Combine(Name, Encoding);
}
