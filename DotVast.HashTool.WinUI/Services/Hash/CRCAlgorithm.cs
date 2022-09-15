//---------------------------------------------------------------------------------------------
// Copyright (c) 2019 Ben Thompson
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

using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Services.Hash;

internal class CRCAlgorithm : HashAlgorithm
{
    #region CRC 算法参数模型

    /// <summary>
    ///     算法名称。
    /// </summary>
    public readonly string Name;

    /// <summary>
    ///     宽度，即 CRC 的比特数。
    /// </summary>
    public readonly int Width;

    /// <summary>
    ///     用于 CRC 计算的多项式，忽略最高位。
    /// </summary>
    public readonly ulong Polynomial;

    /// <summary>
    ///     算法开始时寄存器的初始值。
    /// </summary>
    public readonly ulong Initial;

    /// <summary>
    ///     计算结果与此参数异或后得到最终的 CRC 值。
    /// </summary>
    public readonly ulong OutputXor;

    /// <summary>
    ///     输入是否反转。
    /// </summary>
    public readonly bool IsInputReflected;

    /// <summary>
    ///     输出是否反转。
    /// </summary>
    public readonly bool IsOutputReflected;

    #endregion

    #region 辅助参数

    /// <summary>
    ///     查表法所使用的计算表。
    /// </summary>
    private readonly ulong[] _precomputationTable = new ulong[256];

    /// <summary>
    ///     用于隐藏 64 位工作寄存器中不需要的数据的掩码。
    /// </summary>
    private readonly ulong _mask;

    /// <summary>
    ///     输出反转处理时所使用的参数。
    /// </summary>
    private readonly int _toRight;

    /// <summary>
    ///     截至目前，处理的所有缓冲区的累积 CRC 值。
    /// </summary>
    private ulong _current;

    #endregion

    public CRCAlgorithm(string name, int width, ulong polynomial, ulong initial, bool isInputReflected, bool isOutputReflected, ulong outputXor)
    {
        if (width < 8 || width > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Must be a multiple of 8 and between 8 and 64.");
        }

        Name = name;
        Width = width;
        Polynomial = polynomial;
        Initial = initial;
        IsInputReflected = isInputReflected;
        IsOutputReflected = isOutputReflected;
        OutputXor = outputXor;
        _mask = ulong.MaxValue >> (64 - width);

        CreateLookupTable();

        if (IsOutputReflected == false)
        {
            _toRight = Width < 8 ? 0 : Width - 8;
        }

        Initialize();
    }

    public override void Initialize()
    {
        _current = IsOutputReflected ? ReverseBits(Initial, Width) : Initial;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        if (ibStart < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ibStart));
        }

        if (cbSize < 0 || ibStart + cbSize > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(cbSize));
        }

        if (IsOutputReflected)
        {
            for (var i = ibStart; i < ibStart + cbSize; i++)
            {
                _current = _precomputationTable[(_current ^ array[i]) & 0xFF] ^ (_current >> 8);
            }
        }
        else
        {
            for (var i = ibStart; i < ibStart + cbSize; i++)
            {
                _current = _precomputationTable[((_current >> _toRight) ^ array[i]) & 0xFF] ^ (_current << 8);
            }
        }
    }

    protected override byte[] HashFinal()
    {
        var output = (_current ^ OutputXor) & _mask;

        var result = BitConverter.GetBytes(output);

        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(BitConverter.GetBytes(output));
        }

        Array.Resize(ref result, Width / 8);

        Array.Reverse(BitConverter.GetBytes(output));

        return BitConverter.GetBytes(output);
    }

    private void CreateLookupTable()
    {
        for (var i = 0; i < _precomputationTable.Length; i++)
        {
            var r = (ulong)i;

            if (IsInputReflected)
            {
                r = ReverseBits(r, Width);
            }
            else if (Width > 8)
            {
                r <<= Width - 8;
            }

            var lastBit = 1UL << (Width - 1);

            for (var j = 0; j < 8; j++)
            {
                if ((r & lastBit) != 0)
                {
                    r = (r << 1) ^ Polynomial;
                }
                else
                {
                    r <<= 1;
                }
            }

            if (IsInputReflected)
            {
                r = ReverseBits(r, Width);
            }

            _precomputationTable[i] = r;
        }
    }

    private static ulong ReverseBits(ulong value, int valueLength)
    {
        ulong output = 0;

        for (var i = valueLength - 1; i >= 0; i--)
        {
            output |= (value & 1) << i;
            value >>= 1;
        }

        return output;
    }
}
