using DotVast.HashTool.WinUI.Helpers;

using Crypto = System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Services.Hash;

public class Hash : GenericEnum<string>
{
    public string Name => base._value;

    public Crypto.HashAlgorithm Algorithm
    {
        get;
    }

    // CRC
    public static readonly Hash CRC32 = new("CRC32", CRC.CreateCRC32());

    // MD
    //public static readonly Hash MD4 = "MD4";
    public static readonly Hash MD5 = new("MD5", Crypto.MD5.Create());

    // SHA1
    public static readonly Hash SHA1 = new("SHA1", Crypto.SHA1.Create());

    // SHA2
    //public static readonly Hash SHA224 = "SHA224";
    public static readonly Hash SHA256 = new("SHA256", Crypto.SHA256.Create());
    public static readonly Hash SHA384 = new("SHA384", Crypto.SHA384.Create());
    public static readonly Hash SHA512 = new("SHA512", Crypto.SHA512.Create());

    // SHA3
    //public static readonly Hash SHA3_224 = "SHA3-224";
    //public static readonly Hash SHA3_256 = "SHA3-256";
    //public static readonly Hash SHA3_384 = "SHA3-384";
    //public static readonly Hash SHA3_512 = "SHA3-512";

    // Blake2B
    //public static readonly Hash Blake2B_160 = "Blake2B-160";
    //public static readonly Hash Blake2B_256 = "Blake2B-256";
    //public static readonly Hash Blake2B_384 = "Blake2B-384";
    //public static readonly Hash Blake2B_512 = "Blake2B-512";

    // Blake2S
    //public static readonly Hash Blake2S_128 = "Blake2S-128";
    //public static readonly Hash Blake2S_160 = "Blake2S-160";
    //public static readonly Hash Blake2S_224 = "Blake2S-224";
    //public static readonly Hash Blake2S_256 = "Blake2S-256";

    // Keccak
    //public static readonly Hash Keccak_224 = "Keccak-224";
    //public static readonly Hash Keccak_256 = "Keccak-256";
    //public static readonly Hash Keccak_288 = "Keccak-288";
    //public static readonly Hash Keccak_384 = "Keccak-384";
    //public static readonly Hash Keccak_512 = "Keccak-512";

    // QuickXor
    public static readonly Hash QuickXor = new("QuickXor", new QuickXorHash());

    private Hash(string name, Crypto.HashAlgorithm hashAlgorithm) : base(name)
    {
        Algorithm = hashAlgorithm;
    }
}
