using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashService
{
    HashKind[] HashKinds { get; }

    HashKind? GetHash(string hashName);

    HashKind[] GetHashes(IEnumerable<string> hashNames);

    HashData GetHashData(HashKind hash);

    IEnumerable<HashSetting> FillHashSettings(IEnumerable<HashSetting>? hashSettings);

    /// <summary>
    /// 将提供的设置合并到默认设置.
    /// </summary>
    /// <remarks>
    /// 仅支持 <see cref="HashSetting.IsChecked"/>,
    /// <see cref="HashSetting.IsEnabledForApp"/>,
    /// <see cref="HashSetting.IsEnabledForContextMenu"/>
    /// 属性的合并.
    /// </remarks>
    /// <param name="hashSettings">哈希设置.</param>
    /// <returns>合并后的设置.</returns>
    IEnumerable<HashSetting> MergeHashSettings(IList<HashSetting> hashSettings);
}
