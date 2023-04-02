using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashService
{
    HashKind[] AllHashes { get; }

    HashKind? GetHash(string hashName);

    HashKind[] GetHashes(IEnumerable<string> hashNames);

    HashData GetHashData(HashKind hash);

    IEnumerable<HashSetting> FillHashSettings(IEnumerable<HashSetting>? hashSettings);
}
