using System.IO;

using Microsoft.CodeAnalysis;

namespace DotVast.Toolkit.StringResource.SourceGenerator;

public sealed class StringResourceInfo
{
    public string Namespace { get; }

    public string Name { get; }

    public string ReswPath { get; }

    public string? ExMethed { get; internal set; }

    public string? ExMethedNamespace { get; internal set; }

    public StringResourceInfo(ITypeSymbol type, string path)
    {
        Namespace = type.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : type.ContainingNamespace.ToDisplayString();
        Name = type.Name;
        ReswPath = Path.GetFullPath(path);
    }
}
