using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DotVast.HashTool.WinUI.Models;

public sealed class GitHubRelease
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
            var version = Regex.Match(value, @"^v?([\d\.]+)-?.*$").Groups[1].Value;
            Version = new Version(version);
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
}
