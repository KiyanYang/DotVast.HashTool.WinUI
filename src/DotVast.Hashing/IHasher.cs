// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.Hashing;

public interface IHasher
{
    int HashLengthInBytes { get; }

    void Reset();

    void Append(ReadOnlySpan<byte> source);

    byte[] Finalize();

    byte[] FinalizeAndReset()
    {
        var ret = Finalize();
        Reset();
        return ret;
    }
}
