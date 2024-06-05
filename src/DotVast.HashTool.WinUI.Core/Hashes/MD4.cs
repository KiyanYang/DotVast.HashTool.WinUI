// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.HashTool.WinUI.Core.Hashes;

public sealed class MD4() : BlockHash(HashSizeInBytes, BlockSizeInBytes)
{
    private const int HashSizeInBytes = 16;
    private const int BlockSizeInBytes = 64;

    private uint _a;
    private uint _b;
    private uint _c;
    private uint _d;

    public override void Initialize()
    {
        base.Initialize();

        _a = 0x67452301;
        _b = 0xefcdab89;
        _c = 0x98badcfe;
        _d = 0x10325476;
    }

    protected override byte[] HashFinal()
    {
        _blockBuffer.Span[_blockBuffer.Position] = 0x80;
        var blockBufferPosition = _blockBuffer.Position;
        blockBufferPosition++;

        if (blockBufferPosition + 8 <= BlockSizeInBytes)
        {
            _blockBuffer.Clear(blockBufferPosition, BlockSizeInBytes - blockBufferPosition - 8);
        }
        else
        {
            _blockBuffer.Clear(blockBufferPosition, BlockSizeInBytes - blockBufferPosition);
            ProcessBlock(_blockBuffer);
            _blockBuffer.Clear(0, blockBufferPosition);
        }

        var lengthInBits = (ulong)_processedBytesCount * 8;
        BinaryPrimitives.WriteUInt64LittleEndian(_blockBuffer.Span[^8..], lengthInBits);
        ProcessBlock(_blockBuffer);

        var ret = new byte[HashSizeInBytes];
        BinaryPrimitives.WriteUInt32LittleEndian(ret.AsSpan(0), _a);
        BinaryPrimitives.WriteUInt32LittleEndian(ret.AsSpan(4), _b);
        BinaryPrimitives.WriteUInt32LittleEndian(ret.AsSpan(8), _c);
        BinaryPrimitives.WriteUInt32LittleEndian(ret.AsSpan(12), _d);
        return ret;
    }

    protected override void ProcessBlock(ReadOnlySpan<byte> block)
    {
        var blockInWord = MemoryMarshal.Cast<byte, uint>(block);

        uint aa = _a;
        uint bb = _b;
        uint cc = _c;
        uint dd = _d;

        foreach (var k in (Span<int>)[0, 4, 8, 12])
        {
            aa = Round1Operation(aa, bb, cc, dd, blockInWord[k], 3);
            dd = Round1Operation(dd, aa, bb, cc, blockInWord[k + 1], 7);
            cc = Round1Operation(cc, dd, aa, bb, blockInWord[k + 2], 11);
            bb = Round1Operation(bb, cc, dd, aa, blockInWord[k + 3], 19);
        }

        foreach (int k in (Span<int>)[0, 1, 2, 3])
        {
            aa = Round2Operation(aa, bb, cc, dd, blockInWord[k], 3);
            dd = Round2Operation(dd, aa, bb, cc, blockInWord[k + 4], 5);
            cc = Round2Operation(cc, dd, aa, bb, blockInWord[k + 8], 9);
            bb = Round2Operation(bb, cc, dd, aa, blockInWord[k + 12], 13);
        }

        foreach (int k in (Span<int>)[0, 2, 1, 3])
        {
            aa = Round3Operation(aa, bb, cc, dd, blockInWord[k], 3);
            dd = Round3Operation(dd, aa, bb, cc, blockInWord[k + 8], 9);
            cc = Round3Operation(cc, dd, aa, bb, blockInWord[k + 4], 11);
            bb = Round3Operation(bb, cc, dd, aa, blockInWord[k + 12], 15);
        }

        _a += aa;
        _b += bb;
        _c += cc;
        _d += dd;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Round1Operation(uint a, uint b, uint c, uint d, uint xk, int s) =>
        uint.RotateLeft(a + ((b & c) | (~b & d)) + xk, s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Round2Operation(uint a, uint b, uint c, uint d, uint xk, int s) =>
        uint.RotateLeft(a + ((b & c) | (b & d) | (c & d)) + xk + 0x5A827999, s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Round3Operation(uint a, uint b, uint c, uint d, uint xk, int s) =>
        uint.RotateLeft(a + (b ^ c ^ d) + xk + 0x6ED9EBA1, s);
}
