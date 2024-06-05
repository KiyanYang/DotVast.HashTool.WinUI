// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Core.Hashes;

public abstract class BlockHash : HashAlgorithm
{
    private protected int _processedBytesCount;
    private protected readonly BlockBuffer _blockBuffer;

    public BlockHash(int hashSizeInBytes, int blockSizeInBytes)
    {
        HashSizeValue = hashSizeInBytes * 8;
        _blockBuffer = new BlockBuffer(blockSizeInBytes);
        Initialize();
    }

    public override void Initialize()
    {
        _processedBytesCount = 0;
        _blockBuffer.Position = 0;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        _processedBytesCount += cbSize;
        var data = new ReadOnlySpan<byte>(array, ibStart, cbSize);

        while (!data.IsEmpty)
        {
            var block = _blockBuffer.Update(ref data);
            if (!block.IsEmpty)
            {
                ProcessBlock(block);
            }
        }
    }

    protected abstract void ProcessBlock(ReadOnlySpan<byte> block);
}
