namespace DotVast.HashTool.WinUI.Services.Hash;

internal sealed class CRC
{
    public static CRCAlgorithm CreateCRC32() => new("CRC-32", 32, 0x04C11DB7, 0xFFFFFFFF, true, true, 0xFFFFFFFF);
}
