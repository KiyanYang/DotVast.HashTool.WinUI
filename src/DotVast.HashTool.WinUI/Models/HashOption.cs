// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.ComponentModel;

using DotVast.HashTool.WinUI.Enums;

namespace DotVast.HashTool.WinUI.Models;

public sealed class HashOption : ObservableObject
{
    public HashKind Kind
    {
        get;
        set; // should be `init`, but https://github.com/microsoft/microsoft-ui-xaml/issues/5315
    }

    public HashFormat Format
    {
        get => _format;
        set
        {
            if (_format != value)
            {
                OnPropertyChanging(s_formatChangingEventArgs);
                _format = value;
                OnPropertyChanged(s_formatChangedEventArgs);
            }
        }
    }
    private HashFormat _format;
    private static readonly PropertyChangingEventArgs s_formatChangingEventArgs = new(nameof(Format));
    private static readonly PropertyChangedEventArgs s_formatChangedEventArgs = new(nameof(Format));

    public override string ToString() => $"({Kind}, {Format})";
}
