using DotVast.HashTool.WinUI.Helpers;

using HashLib4CSharp.Checksum;

using static HashLib4CSharp.Base.HashFactory.Checksum.CRC;
using static HashLib4CSharp.Base.HashFactory.Crypto;

using Crypto = System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Services.Hash;

public sealed class Hash : GenericEnum<string>
{
    public string Name => base._value;

    public Crypto.HashAlgorithm Algorithm
    {
        get;
    }

    // CRC
    public static readonly Hash CRC32 = new("CRC32", CreateCRC(CRCModel.CRC32).ToHashAlgorithm());

    // MD
    public static readonly Hash MD4 = new("MD4", CreateMD4().ToHashAlgorithm());
    public static readonly Hash MD5 = new("MD5", Crypto.MD5.Create());

    // SHA1
    public static readonly Hash SHA1 = new("SHA1", Crypto.SHA1.Create());

    // SHA2
    public static readonly Hash SHA224 = new("SHA224", CreateSHA2_224().ToHashAlgorithm());
    public static readonly Hash SHA256 = new("SHA256", Crypto.SHA256.Create());
    public static readonly Hash SHA384 = new("SHA384", Crypto.SHA384.Create());
    public static readonly Hash SHA512 = new("SHA512", Crypto.SHA512.Create());

    // SHA3
    public static readonly Hash SHA3_224 = new("SHA3-224", CreateSHA3_224().ToHashAlgorithm());
    public static readonly Hash SHA3_256 = new("SHA3-256", CreateSHA3_256().ToHashAlgorithm());
    public static readonly Hash SHA3_384 = new("SHA3-384", CreateSHA3_384().ToHashAlgorithm());
    public static readonly Hash SHA3_512 = new("SHA3-512", CreateSHA3_512().ToHashAlgorithm());

    // Blake2B
    public static readonly Hash Blake2B_160 = new("Blake2B-160", CreateBlake2B_160().ToHashAlgorithm());
    public static readonly Hash Blake2B_256 = new("Blake2B-256", CreateBlake2B_256().ToHashAlgorithm());
    public static readonly Hash Blake2B_384 = new("Blake2B-384", CreateBlake2B_384().ToHashAlgorithm());
    public static readonly Hash Blake2B_512 = new("Blake2B-512", CreateBlake2B_512().ToHashAlgorithm());

    // Blake2S
    public static readonly Hash Blake2S_128 = new("Blake2S-128", CreateBlake2S_128().ToHashAlgorithm());
    public static readonly Hash Blake2S_160 = new("Blake2S-160", CreateBlake2S_160().ToHashAlgorithm());
    public static readonly Hash Blake2S_224 = new("Blake2S-224", CreateBlake2S_224().ToHashAlgorithm());
    public static readonly Hash Blake2S_256 = new("Blake2S-256", CreateBlake2S_256().ToHashAlgorithm());

    // Keccak
    public static readonly Hash Keccak_224 = new("Keccak-224", CreateKeccak_224().ToHashAlgorithm());
    public static readonly Hash Keccak_256 = new("Keccak-256", CreateKeccak_256().ToHashAlgorithm());
    public static readonly Hash Keccak_288 = new("Keccak-288", CreateKeccak_288().ToHashAlgorithm());
    public static readonly Hash Keccak_384 = new("Keccak-384", CreateKeccak_384().ToHashAlgorithm());
    public static readonly Hash Keccak_512 = new("Keccak-512", CreateKeccak_512().ToHashAlgorithm());

    // QuickXor
    public static readonly Hash QuickXor = new("QuickXor", new QuickXorHash());

    private Hash(string name, Crypto.HashAlgorithm hashAlgorithm) : base(name)
    {
        Algorithm = hashAlgorithm;
    }
}
