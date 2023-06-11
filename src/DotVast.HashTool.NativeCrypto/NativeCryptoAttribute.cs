// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.NativeCrypto;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class NativeCryptoAttribute : Attribute
{
    public string Feature { get; }
    public string FnPrefix { get; }
    public int HashSizeInBytes { get; }

    public NativeCryptoAttribute(string feature, string fnPrefix, int hashSizeInBytes)
    {
        Feature = feature;
        FnPrefix = fnPrefix;
        HashSizeInBytes = hashSizeInBytes;
    }
}
