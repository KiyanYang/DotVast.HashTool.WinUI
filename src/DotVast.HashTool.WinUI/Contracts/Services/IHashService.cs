// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashService
{
    IReadOnlyList<HashKind> HashKinds { get; }

    string GetName(HashKind hashKind);

    /// <summary>
    /// 从哈希名称获取 <see cref="HashKind"/>.
    /// 在获取时该名称会与 <see cref="HashKind"/> 以及内部哈希配置中的 <c>Name</c>, <c>Alias</c> 进行对比.
    /// </summary>
    /// <param name="hashName">要进行匹配的哈希名称</param>
    /// <returns>与哈希名称匹配的 <see cref="HashKind"/></returns>
    HashKind? GetHashKind(string hashName);

    /// <summary>
    /// 从哈希名称序列获取 <see cref="HashKind"/> 序列.
    /// 在获取时每个名称都会与 <see cref="HashKind"/> 以及内部哈希配置中的 <c>Name</c>, <c>Alias</c> 进行对比.
    /// </summary>
    /// <param name="hashNames">要进行匹配的哈希名称序列</param>
    /// <returns>与哈希名称序列匹配的 <see cref="HashKind"/> 序列</returns>
    IEnumerable<HashKind?> GetHashKinds(IEnumerable<string> hashNames);

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
