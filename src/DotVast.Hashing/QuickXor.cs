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
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotVast.Hashing;

/// <summary>
/// A quick, simple non-cryptographic hash algorithm that works by XORing the bytes in a circular-shifting fashion.
/// 
/// <para>See <see href="https://learn.microsoft.com/onedrive/developer/code-snippets/quickxorhash"/>.</para>
/// </summary>
public sealed class QuickXor : IHasher
{
    private const int HashSizeInBytes = 20;
    private const int BlockLength = 20 * 8 * 11;

    private readonly byte[] _block = new byte[BlockLength];
    private long _lengthSoFar;

    public int HashLengthInBytes => HashSizeInBytes;

    public void Reset()
    {
        Array.Clear(_block);
        _lengthSoFar = 0;
    }

    public void Append(ReadOnlySpan<byte> source)
    {
        while (!source.IsEmpty)
        {
            var blockIndex = (int)(_lengthSoFar % BlockLength);
            var remainning = BlockLength - blockIndex;
            var readLength = Math.Min(remainning, source.Length);
            Xor(_block.AsSpan(blockIndex, readLength), source);
            source = source.Slice(readLength);
            _lengthSoFar += readLength;
        }

        static void Xor(Span<byte> destination, ReadOnlySpan<byte> source)
        {
            Debug.Assert(destination.Length <= source.Length);

            var destinationVector = MemoryMarshal.Cast<byte, Vector<byte>>(destination);
            var sourceVector = MemoryMarshal.Cast<byte, Vector<byte>>(source);

            for (var i = 0; i < destinationVector.Length; i++)
            {
                destinationVector[i] ^= sourceVector[i];
            }

            for (var index = destinationVector.Length * Vector<byte>.Count; index != destination.Length; index++)
            {
                destination[index] ^= source[index];
            }
        }
    }

    public byte[] Finalize()
    {
        Span<byte> hash = stackalloc byte[HashSizeInBytes + 1];
        for (var i = 0; i < BlockLength; i++)
        {
            var shift = i * 11 % 160;
            var shiftBytes = shift / 8;
            var shiftBits = shift % 8;
            var shifted = _block[i] << shiftBits;
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
