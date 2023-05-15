// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.NativeCrypto;

[NativeCrypto("ripemd128", 16)]
public partial class RIPEMD128 : NativeCryptoBase
{
}

[NativeCrypto("ripemd160", 20)]
public partial class RIPEMD160 : NativeCryptoBase
{
}

[NativeCrypto("ripemd256", 32)]
public partial class RIPEMD256 : NativeCryptoBase
{
}

[NativeCrypto("ripemd320", 40)]
public partial class RIPEMD320 : NativeCryptoBase
{
}
