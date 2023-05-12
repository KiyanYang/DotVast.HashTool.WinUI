// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Core.Hashes;

/// <summary>
/// <see href="https://www.oscca.gov.cn/sca/xxgk/2010-12/17/1002389/files/302a3ada057c4a73830536d03e683110.pdf">SM3密码杂凑算法</see>.
/// </summary>
/// <remarks>
/// 优化: SM3 杂凑算法的软件快速实现研究 (DOI: 10.11992/tis.201507036)
/// </remarks>
public sealed class SM3 : HashAlgorithm
{
    private const int HashSizeInBytes = 32;
    private const int BlockSizeInBytes = 64;

    // K{j} = T{j} <<< j
    private static readonly uint[] s_k = new uint[64]
    {
        0x79CC4519u, 0xF3988A32u, 0xE7311465u, 0xCE6228CBu, 0x9CC45197u, 0x3988A32Fu, 0x7311465Eu, 0xE6228CBCu,
        0xCC451979u, 0x988A32F3u, 0x311465E7u, 0x6228CBCEu, 0xC451979Cu, 0x88A32F39u, 0x11465E73u, 0x228CBCE6u,
        0x9D8A7A87u, 0x3B14F50Fu, 0x7629EA1Eu, 0xEC53D43Cu, 0xD8A7A879u, 0xB14F50F3u, 0x629EA1E7u, 0xC53D43CEu,
        0x8A7A879Du, 0x14F50F3Bu, 0x29EA1E76u, 0x53D43CECu, 0xA7A879D8u, 0x4F50F3B1u, 0x9EA1E762u, 0x3D43CEC5u,
        0x7A879D8Au, 0xF50F3B14u, 0xEA1E7629u, 0xD43CEC53u, 0xA879D8A7u, 0x50F3B14Fu, 0xA1E7629Eu, 0x43CEC53Du,
        0x879D8A7Au, 0x0F3B14F5u, 0x1E7629EAu, 0x3CEC53D4u, 0x79D8A7A8u, 0xF3B14F50u, 0xE7629EA1u, 0xCEC53D43u,
        0x9D8A7A87u, 0x3B14F50Fu, 0x7629EA1Eu, 0xEC53D43Cu, 0xD8A7A879u, 0xB14F50F3u, 0x629EA1E7u, 0xC53D43CEu,
        0x8A7A879Du, 0x14F50F3Bu, 0x29EA1E76u, 0x53D43CECu, 0xA7A879D8u, 0x4F50F3B1u, 0x9EA1E762u, 0x3D43CEC5u,
    };

    private readonly uint[] _v = new uint[8];
    private readonly uint[] _w = new uint[68];
    private readonly byte[] _block = new byte[BlockSizeInBytes];
    private long _lengthSoFar;
    private int _currentBlockSize;

    public SM3()
    {
        HashSizeValue = HashSizeInBytes;
        Initialize();
    }

    public override void Initialize()
    {
        _lengthSoFar = 0;
        _currentBlockSize = 0;
        _v[0] = 0x7380166Fu;
        _v[1] = 0x4914B2B9u;
        _v[2] = 0x172442D7u;
        _v[3] = 0xDA8A0600u;
        _v[4] = 0xA96F30BCu;
        _v[5] = 0x163138AAu;
        _v[6] = 0xE38DEE4Du;
        _v[7] = 0xB0FB0E4Eu;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        _lengthSoFar += cbSize;

        while (cbSize > 0)
        {
            var readSize = Math.Min(BlockSizeInBytes - _currentBlockSize, cbSize);

            if (readSize == BlockSizeInBytes)
            {
                CF(new ReadOnlySpan<byte>(array, ibStart, BlockSizeInBytes));
            }
            else
            {
                Array.Copy(array, ibStart, _block, _currentBlockSize, readSize);
                _currentBlockSize = (_currentBlockSize + readSize) % BlockSizeInBytes;
                if (_currentBlockSize == 0)
                {
                    CF(_block);
                }
            }

            ibStart += readSize;
            cbSize -= readSize;
        }
    }

    protected override byte[] HashFinal()
    {
        _block[_currentBlockSize] = 0x80;
        _currentBlockSize++;

        if (_currentBlockSize + 8 <= BlockSizeInBytes)
        {
            Array.Clear(_block, _currentBlockSize, BlockSizeInBytes - _currentBlockSize - 8);
        }
        else
        {
            Array.Clear(_block, _currentBlockSize, BlockSizeInBytes - _currentBlockSize);
            CF(_block);
            Array.Clear(_block, 0, _currentBlockSize);
        }

        BinaryPrimitives.WriteUInt64BigEndian(_block.AsSpan(BlockSizeInBytes - 8), (ulong)_lengthSoFar * 8);
        CF(_block);

        var ret = new byte[32];
        WriteBigEndian(_v, MemoryMarshal.Cast<byte, uint>(ret));
        return ret;
    }

    void CF(ReadOnlySpan<byte> block)
    {
        #region MessageExpansion 0..19

        // W[0]W[1]..W[15] = B(i)
        // FOR j = 16 TO 67
        //     W[j] = P1(W[j-16] ^ W[j-9] ^ (W[j-3]<<<15)) ^ (W[j-13]<<<7) ^ W[j-6]
        // ENDFOR
        // FOR j = 0 TO 63
        //     W[j]' = W[j] ^ W[j+4]
        // ENDFOR

        WriteBigEndian(MemoryMarshal.Cast<byte, uint>(block), _w); // 0..15
        W16(16);
        W16(17);
        W16(18);
        W16(19);

        #endregion MessageExpansion 0..19

        var a = _v[0];
        var b = _v[1];
        var c = _v[2];
        var d = _v[3];
        var e = _v[4];
        var f = _v[5];
        var g = _v[6];
        var h = _v[7];
        uint ss1, ss2;

        var j = 0;
        while (j < 16)
        {
            ss2 = uint.RotateLeft(a, 12);
            ss1 = uint.RotateLeft(ss2 + e + s_k[j], 7);
            ss2 ^= ss1;
            d = FF00(a, b, c) + d + ss2 + (_w[j] ^ _w[j + 4]);
            h = GG00(e, f, g) + h + ss1 + _w[j];
            h = P0(h);
            b = uint.RotateLeft(b, 9);
            f = uint.RotateLeft(f, 19);
            j++;

            ss2 = uint.RotateLeft(d, 12);
            ss1 = uint.RotateLeft(ss2 + h + s_k[j], 7);
            ss2 ^= ss1;
            c = FF00(d, a, b) + c + ss2 + (_w[j] ^ _w[j + 4]);
            g = GG00(h, e, f) + g + ss1 + _w[j];
            g = P0(g);
            a = uint.RotateLeft(a, 9);
            e = uint.RotateLeft(e, 19);
            j++;

            ss2 = uint.RotateLeft(c, 12);
            ss1 = uint.RotateLeft(ss2 + g + s_k[j], 7);
            ss2 ^= ss1;
            b = FF00(c, d, a) + b + ss2 + (_w[j] ^ _w[j + 4]);
            f = GG00(g, h, e) + f + ss1 + _w[j];
            f = P0(f);
            d = uint.RotateLeft(d, 9);
            h = uint.RotateLeft(h, 19);
            j++;

            ss2 = uint.RotateLeft(b, 12);
            ss1 = uint.RotateLeft(ss2 + f + s_k[j], 7);
            ss2 ^= ss1;
            a = FF00(b, c, d) + a + ss2 + (_w[j] ^ _w[j + 4]);
            e = GG00(f, g, h) + e + ss1 + _w[j];
            e = P0(e);
            c = uint.RotateLeft(c, 9);
            g = uint.RotateLeft(g, 19);
            j++;
        }
        while (j < 64)
        {
            W16(j + 4);
            ss2 = uint.RotateLeft(a, 12);
            ss1 = uint.RotateLeft(ss2 + e + s_k[j], 7);
            ss2 ^= ss1;
            d = FF16(a, b, c) + d + ss2 + (_w[j] ^ _w[j + 4]);
            h = GG16(e, f, g) + h + ss1 + _w[j];
            h = P0(h);
            b = uint.RotateLeft(b, 9);
            f = uint.RotateLeft(f, 19);
            j++;

            W16(j + 4);
            ss2 = uint.RotateLeft(d, 12);
            ss1 = uint.RotateLeft(ss2 + h + s_k[j], 7);
            ss2 ^= ss1;
            c = FF16(d, a, b) + c + ss2 + (_w[j] ^ _w[j + 4]);
            g = GG16(h, e, f) + g + ss1 + _w[j];
            g = P0(g);
            a = uint.RotateLeft(a, 9);
            e = uint.RotateLeft(e, 19);
            j++;

            W16(j + 4);
            ss2 = uint.RotateLeft(c, 12);
            ss1 = uint.RotateLeft(ss2 + g + s_k[j], 7);
            ss2 ^= ss1;
            b = FF16(c, d, a) + b + ss2 + (_w[j] ^ _w[j + 4]);
            f = GG16(g, h, e) + f + ss1 + _w[j];
            f = P0(f);
            d = uint.RotateLeft(d, 9);
            h = uint.RotateLeft(h, 19);
            j++;

            W16(j + 4);
            ss2 = uint.RotateLeft(b, 12);
            ss1 = uint.RotateLeft(ss2 + f + s_k[j], 7);
            ss2 ^= ss1;
            a = FF16(b, c, d) + a + ss2 + (_w[j] ^ _w[j + 4]);
            e = GG16(f, g, h) + e + ss1 + _w[j];
            e = P0(e);
            c = uint.RotateLeft(c, 9);
            g = uint.RotateLeft(g, 19);
            j++;
        }

        Unsafe.As<uint, Vector256<uint>>(ref _v[0]) ^= Vector256.Create(a, b, c, d, e, f, g, h);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void W16(int i) =>
        _w[i] = P1(uint.RotateLeft(_w[i - 3], 15) ^ _w[i - 9] ^ _w[i - 16]) ^ uint.RotateLeft(_w[i - 13], 7) ^ _w[i - 6];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void WriteBigEndian(ReadOnlySpan<uint> source, Span<uint> destination)
    {
        if (BitConverter.IsLittleEndian)
        {
#if NET8_0_OR_GREATER
            BinaryPrimitives.ReverseEndianness(source, destination);
#else
            if (destination.Length < source.Length)
            {
                throw new ArgumentException("The destination's length is too small.");
            }

            for (var i = 0; i < source.Length; i++)
            {
                destination[i] = BinaryPrimitives.ReverseEndianness(source[i]);
            }
#endif
        }
        else
        {
            source.CopyTo(destination);
        }
    }

    #region Transformations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint FF00(uint x, uint y, uint z) =>
        x ^ y ^ z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint FF16(uint x, uint y, uint z) =>
        (x & y) | (x & z) | (y & z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint GG00(uint x, uint y, uint z) =>
        x ^ y ^ z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint GG16(uint x, uint y, uint z) =>
        ((y ^ z) & x) ^ z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint P0(uint x) =>
        x ^ uint.RotateLeft(x, 9) ^ uint.RotateLeft(x, 17);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static uint P1(uint x) =>
        x ^ uint.RotateLeft(x, 15) ^ uint.RotateLeft(x, 23);

    #endregion Transformations
}
