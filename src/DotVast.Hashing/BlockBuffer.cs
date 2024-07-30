// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.Hashing;

internal sealed class BlockBuffer(int size)
{
    private readonly byte[] _data = new byte[size];
    private readonly int _size = size;

    internal int Position { get; set; }

    internal Span<byte> Span => _data.AsSpan();
    public static implicit operator ReadOnlySpan<byte>(BlockBuffer buffer) => buffer._data;

    /// <summary>
    /// 增添数据。
    /// </summary>
    /// <param name="source">原始数据。</param>
    /// <returns>待处理的数据块。若无，则返回<![CDATA[ReadOnlySpan<byte>.Empty]]>。</returns>
    internal ReadOnlySpan<byte> Update(ref ReadOnlySpan<byte> source)
    {
        var len = Math.Min(_size - Position, source.Length);

        // 创建待处理数据，并更新原始数据
        var pendingBlock = source.Slice(0, len);
        source = source.Slice(len);
        //UpdatedLength += len;

        // 此时 Position == 0 且 source.Length >= _size(一个数据块长)，不进行复制，直接返回待处理数据块。
        if (len == _size)
        {
            return pendingBlock;
        }

        // 此时 Position != 0 或 source.Length < _size(一个数据块长)，需要进行复制
        pendingBlock.CopyTo(_data.AsSpan(Position));
        Position += len;

        // 若恰好填满 _data，则返回 _data 作为待处理数据块，否则返回空数据块
        if (Position == _size)
        {
            Position = 0;
            return _data;
        }

        return [];
    }

    internal void Clear(int index, int length)
    {
        Array.Clear(_data, index, length);
    }
}
