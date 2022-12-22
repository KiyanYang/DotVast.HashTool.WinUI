using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class GitHubRelease
{
    /// <summary>
    /// 网址.
    /// </summary>
    [JsonPropertyName("html_url")]
    public string Url { get; set; } = string.Empty;

    private string _tagName = string.Empty;

    /// <summary>
    /// 标签.
    /// </summary>
    [JsonPropertyName("tag_name")]
    public string TagName
    {
        get => _tagName;
        set
        {
            _tagName = value;

            var tagMatch = TagRegex().Match(value);
            var major = int.Parse(tagMatch.Groups["major"].Value);
            var minor = int.Parse(tagMatch.Groups["minor"].Value);
            var patch = int.Parse(tagMatch.Groups["patch"].Value);
            Version = new Version(major, minor, patch);
        }
    }

    /// <summary>
    /// 是否为预发布版本.
    /// </summary>
    [JsonPropertyName("prerelease")]
    public bool IsPreRelease { get; set; }

    [JsonIgnore]
    public Version? Version { get; set; }

    /// <summary>
    /// 发布时间.
    /// </summary>
    [JsonPropertyName("published_at")]
    public DateTime PublishAt { get; set; }

    /// <summary>
    /// 标题.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 发布说明.
    /// </summary>
    [JsonPropertyName("body")]
    public string Description { get; set; } = string.Empty;

    [GeneratedRegex(@"^v(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(?:-(?:alpha|beta|rc)(?:\.\d+)?)?$")]
    private static partial Regex TagRegex();
}
