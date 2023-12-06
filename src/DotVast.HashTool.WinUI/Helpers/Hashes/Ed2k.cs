// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Helpers.Hashes;

internal sealed class Ed2k : HashAlgorithm
{
    private const int Ed2kChunkSize = 9728000;

    private readonly HashAlgorithm _md4 = HashKind.MD4.ToHashAlgorithm();
    private readonly List<byte> _chunkHashes = new(16);
    private int _currentChunkSize;

    public Ed2k()
    {
        HashSizeValue = _md4.HashSize;
    }

    public override void Initialize()
    {
        _md4.Initialize();
        _chunkHashes.Clear();
        _currentChunkSize = 0;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        while (cbSize > 0)
        {
            var readSize = Math.Min(Ed2kChunkSize - _currentChunkSize, cbSize);
            _currentChunkSize = (_currentChunkSize + readSize) % Ed2kChunkSize;
            if (_currentChunkSize == 0)
            {
                _md4.TransformFinalBlock(array, ibStart, readSize);
                _chunkHashes.AddRange(_md4.Hash!);
            }
            else
            {
                _md4.TransformBlock(array, ibStart, readSize, null, 0);
            }
            ibStart += readSize;
            cbSize -= readSize;
        }
    }

    protected override byte[] HashFinal()
    {
        _md4.TransformFinalBlock([], 0, 0);
        if (_chunkHashes.Count == 0)
        {
            return _md4.Hash!;
        }
        else
        {
            _chunkHashes.AddRange(_md4.Hash!);
            return _md4.ComputeHash([.. _chunkHashes]);
        }
    }

    #region Finalizer, IDisposable

    private bool _disposed = false;

    ~Ed2k() => Dispose(false);

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _md4.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion Finalizer, IDisposable
}
