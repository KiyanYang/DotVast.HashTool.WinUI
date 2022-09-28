using Microsoft.CodeAnalysis;

namespace DotVast.Toolkit.StringResource.SourceGenerator;

internal static class ISymbolExtensions
{
    public static bool HasFullyQualifiedName(this ISymbol symbol, string name) =>
        symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == name;
}
