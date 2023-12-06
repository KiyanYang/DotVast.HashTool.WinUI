// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.ComponentModel;
using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashResult(string path, IReadOnlyList<HashResultItem> data)
{
    /// <summary>
    /// 文件路径.
    /// </summary>
    public string Path { get; } = path;

    /// <summary>
    /// 哈希结果.
    /// </summary>
    public IReadOnlyList<HashResultItem> Data { get; } = data;
}

public sealed class HashResultItem : ObservableObject
{
    private readonly HashOption _hashOption;
    private readonly byte[] _hashValue;

    public HashResultItem(HashOption hashOption, byte[] hashValue)
    {
        _hashOption = hashOption;
        _hashValue = hashValue;
        _value = GetFormattedHash(_hashValue, hashOption.Format);
        _hashOption.PropertyChanged += HashOption_PropertyChanged;
    }

    ~HashResultItem() => _hashOption.PropertyChanged -= HashOption_PropertyChanged;

    /// <summary>
    /// 名称.
    /// </summary>
    [JsonIgnore]
    public string Name => Kind.ToName();

    public HashKind Kind => _hashOption.Kind;

    public string Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                OnPropertyChanging(s_valueChangingEventArgs);
                _value = value;
                OnPropertyChanged(s_valueChangedEventArgs);
            }
        }
    }
    private string _value;
    private static readonly PropertyChangingEventArgs s_valueChangingEventArgs = new(nameof(Value));
    private static readonly PropertyChangedEventArgs s_valueChangedEventArgs = new(nameof(Value));

    private void HashOption_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is HashOption option && e.PropertyName == nameof(HashOption.Format))
        {
            Value = GetFormattedHash(_hashValue, option.Format);
        }
    }

    private static string GetFormattedHash(byte[] hashData, HashFormat format)
    {
        return format switch
        {
            HashFormat.Base16Upper => Convert.ToHexString(hashData),
            HashFormat.Base16Lower => Core.Helpers.Convert.ToLowerHexString(hashData),
            HashFormat.Base64 => Convert.ToBase64String(hashData),
            _ => throw new ArgumentOutOfRangeException(nameof(format), $"The HashKind {format} is out of range and cannot be processed."),
        };
    }
}
