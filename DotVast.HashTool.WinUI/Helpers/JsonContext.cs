using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Helpers;

/// <summary>
/// 公用 JsonSerializerContext.
/// </summary>
[JsonSerializable(typeof(IList<Hash>))]
[JsonSerializable(typeof(GitHubRelease))]
[JsonSerializable(typeof(GitHubRelease[]))]
public sealed partial class JsonContext : JsonSerializerContext
{
}
