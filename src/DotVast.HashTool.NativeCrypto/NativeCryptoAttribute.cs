// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.NativeCrypto;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class NativeCryptoAttribute : Attribute
{
    public string FnPrefix { get; }
    public int HashSizeInBytes { get; }

    public NativeCryptoAttribute(string fnPrefix, int hashSizeInBytes)
    {
        FnPrefix = fnPrefix;
        HashSizeInBytes = hashSizeInBytes;
    }
}
