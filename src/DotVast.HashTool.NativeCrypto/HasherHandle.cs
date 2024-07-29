// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace DotVast.HashTool.NativeCrypto;

internal abstract class HasherHandle() : SafeHandle(nint.Zero, true)
{
    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        Free();
        SetHandle(nint.Zero);
        return true;
    }

    protected abstract void Free();
}
