//---------------------------------------------------------------------------------------------
// Copyright (c) 2016 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//---------------------------------------------------------------------------------------------

namespace DotVast.HashTool.WinUI.Services.Hash;

/// <summary>
/// A quick, simple non-cryptographic hash algorithm that works by XORing the bytes in a circular-shifting fashion.
///
/// A high level description of the algorithm without the introduction of the length is as follows:
///
/// Let's say a "block" is a 160 bit block of bits (e.g. byte[20]).
///
///   method block zero():
///     returns a block with all zero bits.
///
///   method block extend8(byte b):
///     returns a block with all zero bits except for the lower 8 bits which come from b.
///
///   method block extend64(int64 i):
///     returns a block of all zero bits except for the lower 64 bits which come from i.
///
///   method block rotate(block bl, int n):
///     returns bl rotated left by n bits.
///
///   method block xor(block bl1, block bl2):
///     returns a bitwise xor of bl1 with bl2
///
///   method block XorHash0(byte rgb[], int cb):
///     block ret = zero()
///     for (int i = 0; i &lt; cb; i ++)
///       ret = xor(ret, rotate(extend8(rgb[i]), i * 11))
///     returns ret
///
///   entrypoint block XorHash(byte rgb[], int cb):
///     returns xor(extend64(cb), XorHash0(rgb, cb))
///
/// The final hash should xor the length of the data with the least significant bits of the resulting block.
/// </summary>
internal class QuickXorHash : System.Security.Cryptography.HashAlgorithm
{
    private const int BitsInLastCell = 32;
    private const byte Shift = 11;
    private const byte WidthInBits = 160;

    private ulong[] _data = Array.Empty<ulong>();
    private long _lengthSoFar;
    private int _shiftSoFar;

    public QuickXorHash()
    {
        Initialize();
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        unchecked
        {
            var currentShift = _shiftSoFar;

            // The bitvector where we'll start xoring
            var vectorArrayIndex = currentShift / 64;

            // The position within the bit vector at which we begin xoring
            var vectorOffset = currentShift % 64;
            var iterations = Math.Min(cbSize, QuickXorHash.WidthInBits);

            for (var i = 0; i < iterations; i++)
            {
                var isLastCell = vectorArrayIndex == _data.Length - 1;
                var bitsInVectorCell = isLastCell ? QuickXorHash.BitsInLastCell : 64;

                // There's at least 2 bitvectors before we reach the end of the array
                if (vectorOffset <= bitsInVectorCell - 8)
                {
                    for (var j = ibStart + i; j < cbSize + ibStart; j += QuickXorHash.WidthInBits)
                    {
                        _data[vectorArrayIndex] ^= (ulong)array[j] << vectorOffset;
                    }
                }
                else
                {
                    var index1 = vectorArrayIndex;
                    var index2 = isLastCell ? 0 : (vectorArrayIndex + 1);
                    var low = (byte)(bitsInVectorCell - vectorOffset);

                    byte xoredByte = 0;
                    for (var j = ibStart + i; j < cbSize + ibStart; j += QuickXorHash.WidthInBits)
                    {
                        xoredByte ^= array[j];
                    }
                    _data[index1] ^= (ulong)xoredByte << vectorOffset;
                    _data[index2] ^= (ulong)xoredByte >> low;
                }
                vectorOffset += QuickXorHash.Shift;
                while (vectorOffset >= bitsInVectorCell)
                {
                    vectorArrayIndex = isLastCell ? 0 : vectorArrayIndex + 1;
                    vectorOffset -= bitsInVectorCell;
                }
            }

            // Update the starting position in a circular shift pattern
            _shiftSoFar = (_shiftSoFar + QuickXorHash.Shift * (cbSize % QuickXorHash.WidthInBits)) % QuickXorHash.WidthInBits;
        }

        _lengthSoFar += cbSize;
    }

    protected override byte[] HashFinal()
    {
        // Create a byte array big enough to hold all our data
        var rgb = new byte[(WidthInBits - 1) / 8 + 1];

        // Block copy all our bitvectors to this byte array
        for (var i = 0; i < _data.Length - 1; i++)
        {
            Buffer.BlockCopy(
                BitConverter.GetBytes(_data[i]), 0,
                rgb, i * 8,
                8);
        }

        Buffer.BlockCopy(
            BitConverter.GetBytes(_data[^1]), 0,
            rgb, (_data.Length - 1) * 8,
            rgb.Length - (_data.Length - 1) * 8);

        // XOR the file length with the least significant bits
        // Note that GetBytes is architecture-dependent, so care should
        // be taken with porting. The expected value is 8-bytes in length in little-endian format
        var lengthBytes = BitConverter.GetBytes(_lengthSoFar);
        System.Diagnostics.Debug.Assert(lengthBytes.Length == 8);
        for (var i = 0; i < lengthBytes.Length; i++)
        {
            rgb[(QuickXorHash.WidthInBits / 8) - lengthBytes.Length + i] ^= lengthBytes[i];
        }

        return rgb;
    }

    public override sealed void Initialize()
    {
        _data = new ulong[(QuickXorHash.WidthInBits - 1) / 64 + 1];
        _shiftSoFar = 0;
        _lengthSoFar = 0;
    }

    public override int HashSize => QuickXorHash.WidthInBits;
}
