// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

using DotVast.Hashing;
using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal sealed class Ed2k : IHasher
{
    private const int Ed2kChunkSize = 9728000;

    private readonly IHasher _md4 = HashKind.MD4.ToIHasher();
    private readonly List<byte> _chunkHashes = new(16);
    private int _currentChunkSize;

    public void Reset()
    {
        _md4.Reset();
        _chunkHashes.Clear();
        _currentChunkSize = 0;
    }

    public void Append(ReadOnlySpan<byte> source)
    {
        while (!source.IsEmpty)
        {
            var readSize = Math.Min(Ed2kChunkSize - _currentChunkSize, source.Length);
            _currentChunkSize = (_currentChunkSize + readSize) % Ed2kChunkSize;
            _md4.Append(source[..readSize]);
            if (_currentChunkSize == 0)
            {
                _chunkHashes.AddRange(_md4.FinalizeAndReset());
            }
            source = source.Slice(readSize);
        }
    }

    public byte[] Finalize()
    {
        if (_chunkHashes.Count == 0)
        {
            return _md4.Finalize();
        }
        else
        {
            _chunkHashes.AddRange(_md4.FinalizeAndReset());
            _md4.Append(CollectionsMarshal.AsSpan(_chunkHashes));
            return _md4.Finalize();
        }
    }

    public int HashLengthInBytes => 16;
}
