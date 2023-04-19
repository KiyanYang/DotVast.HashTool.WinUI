// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashService
{
    IReadOnlyList<HashKind> HashKinds { get; }

    string GetName(HashKind hashKind);

    HashKind? GetHash(string hashName);

    HashKind[] GetHashes(IEnumerable<string> hashNames);

    /// <summary>
    /// 获取合并后的哈希设置.
    /// </summary>
    /// <remarks>
    /// 仅支持 <see cref="HashSetting.IsChecked"/>,
    /// <see cref="HashSetting.IsEnabledForApp"/>,
    /// <see cref="HashSetting.IsEnabledForContextMenu"/>
    /// 属性的合并.
    /// </remarks>
    /// <param name="hashSettings">要合并的哈希设置.</param>
    /// <returns>合并后的哈希设置.</returns>
    IEnumerable<HashSetting> GetMergedHashSettings(IList<HashSetting> hashSettings);
}
