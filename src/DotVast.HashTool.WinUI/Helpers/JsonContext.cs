using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Helpers;

/// <summary>
/// 公用 JsonSerializerContext.
/// </summary>
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(TimeSpan))]

[JsonSerializable(typeof(IList<HashKind>))]
[JsonSerializable(typeof(HashTaskMode))]
[JsonSerializable(typeof(HashTaskState))]
[JsonSerializable(typeof(IEnumerable< HashTask>))]
[JsonSerializable(typeof(GitHubRelease))]
[JsonSerializable(typeof(GitHubRelease[]))]
[JsonSerializable(typeof(ObservableCollection<HashResult>))]
public sealed partial class JsonContext : JsonSerializerContext
{
}
