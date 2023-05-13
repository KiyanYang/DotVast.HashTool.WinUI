// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.
//
// Based on namazso/QuickXorHash (https://github.com/namazso/QuickXorHash)
// under BSD Zero Clause License.
//
// Copyright (c) 2022 namazso <admin@namazso.eu>
//
// Permission to use, copy, modify, and/or distribute this software for any
// purpose with or without fee is hereby granted.
//
// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
// REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
// INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
// LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
// OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
// PERFORMANCE OF THIS SOFTWARE.

using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Core.Hashes;

/// <summary>
/// A quick, simple non-cryptographic hash algorithm that works by XORing the bytes in a circular-shifting fashion.
///
/// A high level description of the algorithm without the introduction of the length is as follows:
///
/// <code><![CDATA[
/// Let's say a "block" is a 160 bit block of bits (e.g. byte[20]).
///
///   method block zero():
///     returns a block with all zero bits.
///
///   method block reverse(block b)
///     returns a block with all of the bytes reversed (00010203... => ...03020100)
///
///   method block extend8(byte b):
///     returns a block with all zero bits except for the lower 8 bits which come from b.
///
///   method block extend64(int64 i):
///     returns a block of all zero bits except for the lower 64 bits which come from i and are in little-endian byte order.
///
///   method block rotate(block bl, int n):
///     returns bl rotated left by n bits.
///
///   method block xor(block bl1, block bl2):
///     returns a bitwise xor of bl1 with bl2
///
///   method block XorHash0(byte[] rgb):
///     block ret = zero()
///     for (int i = 0; i < rgb.Length; i ++)
///       ret = xor(ret, rotate(extend8(rgb[i]), i * 11))
///     returns reverse(ret)
///
///   entrypoint block XorHash(byte[] rgb):
///     returns xor(extend64(rgb.Length), XorHash0(rgb))
///
/// The final hash should xor the length of the data with the least significant bits of the resulting block.
/// ]]></code>
/// </summary>
public sealed class QuickXor : HashAlgorithm
{
    private const int HashSizeInBytes = 20;
    private const int HashSizeInBits = HashSizeInBytes * 8;
    private const byte Shift = 11;
    private const int BlockExLength = HashSizeInBits * Shift;

    private readonly byte[] _blockEx = new byte[BlockExLength];
    private long _lengthSoFar;

    private int _BlockExIndex
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (int)(_lengthSoFar % BlockExLength);
    }

    public QuickXor()
    {
        HashSizeValue = HashSizeInBytes;
        Initialize();
    }

    public override void Initialize()
    {
        Array.Clear(_blockEx);
        _lengthSoFar = 0;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        var count = Vector<byte>.Count;
        while (cbSize > count)
        {
            var blockExVecSpan = MemoryMarshal.Cast<byte, Vector<byte>>(_blockEx.AsSpan(_BlockExIndex));
            var arrayVecSpan = MemoryMarshal.Cast<byte, Vector<byte>>(array.AsSpan(ibStart, cbSize));
            var length = Math.Min(blockExVecSpan.Length, arrayVecSpan.Length);
            for (var i = 0; i < length; i++)
            {
                blockExVecSpan[i] ^= arrayVecSpan[i];
            }
            _lengthSoFar += length * count;
            ibStart += length * count;
            cbSize -= length * count;

            while (_BlockExIndex != 0 && cbSize > 0)
            {
                _blockEx[_BlockExIndex] ^= array[ibStart];
                _lengthSoFar++;
                ibStart++;
                cbSize--;
            }
        }
        while (cbSize > 0)
        {
            _blockEx[_BlockExIndex] ^= array[ibStart];
            _lengthSoFar++;
            ibStart++;
            cbSize--;
        }
    }

    protected override byte[] HashFinal()
    {
        Span<byte> hash = stackalloc byte[HashSizeInBytes + 1];
        for (var i = 0; i < BlockExLength; i++)
        {
            var shift = i * Shift % HashSizeInBits;
            var shiftBytes = shift / 8;
            var shiftBits = shift % 8;
            var shifted = _blockEx[i] << shiftBits;
            hash[shiftBytes] ^= (byte)shifted;
            hash[shiftBytes + 1] ^= (byte)(shifted >> 8);
        }
        hash[0] ^= hash[20];

        Unsafe.As<byte, long>(ref hash[12]) ^= BitConverter.IsLittleEndian
            ? _lengthSoFar
            : BinaryPrimitives.ReverseEndianness(_lengthSoFar);

        return hash[..HashSizeInBytes].ToArray();
    }
}

// First, convert the original version to C#.
// Second, perform low-level optimization with Vector<byte>.
//
// internal class QuickXor : HashAlgorithm
// {
//     private const int HashSizeInBytes = 20;
//     private const int HashSizeInBits = HashSizeInBytes * 8;
//     private const byte Shift = 11;
//     private const int BlockExLength = HashSizeInBits * Shift;
//
//     private readonly byte[] _blockEx = new byte[BlockExLength];
//     private long _lengthSoFar;
//
//     public QuickXor()
//     {
//         HashSizeValue = HashSizeInBits;
//         Initialize();
//     }
//
//     public override void Initialize()
//     {
//         Array.Clear(_blockEx);
//         _lengthSoFar = 0;
//     }
//
//     private void XorBlock(Span<byte> data)
//     {
//         for (int i = 0; i < BlockExLength; i++)
//         {
//             _blockEx[i] ^= data[i];
//         }
//     }
//
//     protected override void HashCore(byte[] array, int ibStart, int cbSize)
//     {
//         int i = 0;
//         while (_lengthSoFar % BlockExLength != 0 && i < cbSize)
//         {
//             _blockEx[_lengthSoFar % BlockExLength] ^= array[ibStart + i];
//             _lengthSoFar++;
//             i++;
//         }
//         if (i == cbSize)
//         {
//             return;
//         }
//         while (cbSize - i >= BlockExLength)
//         {
//             XorBlock(array.AsSpan(i, BlockExLength));
//             _lengthSoFar += BlockExLength;
//             i += BlockExLength;
//         }
//         while (i < cbSize)
//         {
//             _blockEx[_lengthSoFar % BlockExLength] ^= array[ibStart + i];
//             _lengthSoFar++;
//             i++;
//         }
//     }
//
//     protected override byte[] HashFinal()
//     {
//         var output = new byte[HashSizeInBytes];
//         byte[] hash = new byte[HashSizeInBytes + 1];
//         for (int i = 0; i < BlockExLength; i++)
//         {
//             int shift = (i * 11) % 160;
//             int shift_bytes = shift / 8;
//             int shift_bits = shift % 8;
//             int shifted = _blockEx[i] << shift_bits;
//             hash[shift_bytes] ^= (byte)shifted;
//             hash[shift_bytes + 1] ^= (byte)(shifted >> 8);
//         }
//         hash[0] ^= hash[20];
//         for (int i = 0; i < 20; i++)
//         {
//             output[i] = hash[i];
//         }
//
//         output[12] ^= (byte)(_lengthSoFar >> 0);
//         output[13] ^= (byte)(_lengthSoFar >> 8);
//         output[14] ^= (byte)(_lengthSoFar >> 16);
//         output[15] ^= (byte)(_lengthSoFar >> 24);
//         output[16] ^= (byte)(_lengthSoFar >> 32);
//         output[17] ^= (byte)(_lengthSoFar >> 40);
//         output[18] ^= (byte)(_lengthSoFar >> 48);
//         output[19] ^= (byte)(_lengthSoFar >> 56);
//
//         return output;
//     }
// }
