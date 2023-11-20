// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.NativeCrypto;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class NativeCryptoAttribute(string feature, string fnPrefix, int hashSizeInBytes) : Attribute
{
    public string Feature { get; } = feature;
    public string FnPrefix { get; } = fnPrefix;
    public int HashSizeInBytes { get; } = hashSizeInBytes;
}
