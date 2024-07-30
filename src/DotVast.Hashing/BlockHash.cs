// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.Hashing;

public abstract class BlockHash : IHasher
{
    private protected int _processedBytesCount;
    private protected readonly BlockBuffer _blockBuffer;

    public int HashLengthInBytes { get; private init; }

    public BlockHash(int hashLengthInBytes, int blockSizeInBytes)
    {
        HashLengthInBytes = hashLengthInBytes;
        _blockBuffer = new BlockBuffer(blockSizeInBytes);
        Reset();
    }

    public void Reset()
    {
        _processedBytesCount = 0;
        _blockBuffer.Position = 0;
        //_blockBuffer.Clear(0, _blockBuffer.Span.Length);
        ResetCore();
    }

    private protected abstract void ResetCore();

    public void Append(ReadOnlySpan<byte> source)
    {
        _processedBytesCount += source.Length;

        while (!source.IsEmpty)
        {
            var block = _blockBuffer.Update(ref source);
            if (!block.IsEmpty)
            {
                ProcessBlock(block);
            }
        }
    }

    private protected abstract void ProcessBlock(ReadOnlySpan<byte> block);

    public abstract byte[] Finalize();
}
