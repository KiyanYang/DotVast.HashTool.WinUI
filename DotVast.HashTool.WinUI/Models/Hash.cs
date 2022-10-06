using System.Text.Json;
using System.Text.Json.Serialization;

using CryptoBase.Digests.SM3;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models.Hashes;

using HashLib4CSharp.Checksum;

using static HashLib4CSharp.Base.HashFactory.Checksum.CRC;
using static HashLib4CSharp.Base.HashFactory.Crypto;

using Crypto = System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Models;

[JsonConverter(typeof(HashJsonConverter))]
public sealed class Hash : GenericEnum<string>
{
    public string Name => _value;

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

    // SM
    public static readonly Hash SM3 = new("SM3", new SM3Digest().ToHashAlgorithm());

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

    private sealed class HashJsonConverter : JsonConverter<Hash>
    {
        public override Hash? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                var name when name == CRC32.Name => CRC32,
                var name when name == MD4.Name => MD4,
                var name when name == MD5.Name => MD5,
                var name when name == SHA1.Name => SHA1,
                var name when name == SHA224.Name => SHA224,
                var name when name == SHA256.Name => SHA256,
                var name when name == SHA384.Name => SHA384,
                var name when name == SHA512.Name => SHA512,
                var name when name == SHA3_224.Name => SHA3_224,
                var name when name == SHA3_256.Name => SHA3_256,
                var name when name == SHA3_384.Name => SHA3_384,
                var name when name == SHA3_512.Name => SHA3_512,
                var name when name == SM3.Name => SM3,
                var name when name == Blake2B_160.Name => Blake2B_160,
                var name when name == Blake2B_256.Name => Blake2B_256,
                var name when name == Blake2B_384.Name => Blake2B_384,
                var name when name == Blake2B_512.Name => Blake2B_512,
                var name when name == Blake2S_128.Name => Blake2S_128,
                var name when name == Blake2S_160.Name => Blake2S_160,
                var name when name == Blake2S_224.Name => Blake2S_224,
                var name when name == Blake2S_256.Name => Blake2S_256,
                var name when name == Keccak_224.Name => Keccak_224,
                var name when name == Keccak_256.Name => Keccak_256,
                var name when name == Keccak_288.Name => Keccak_288,
                var name when name == Keccak_384.Name => Keccak_384,
                var name when name == Keccak_512.Name => Keccak_512,
                var name when name == QuickXor.Name => QuickXor,
                _ => null,
            };
        }
        public override void Write(Utf8JsonWriter writer, Hash value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
