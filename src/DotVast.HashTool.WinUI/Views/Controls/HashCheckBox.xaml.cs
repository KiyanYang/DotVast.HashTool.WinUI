// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views.Controls;

public sealed partial class HashCheckBox : UserControl
{
    private readonly Guid _guid = Guid.NewGuid();

    public HashSetting Setting
    {
        get { return (HashSetting)GetValue(SettingProperty); }
        set { SetValue(SettingProperty, value); }
    }

    public static readonly DependencyProperty SettingProperty =
        DependencyProperty.Register(nameof(Setting), typeof(HashSetting), typeof(HashCheckBox), new PropertyMetadata(null));

    public HashCheckBox()
    {
        InitializeComponent();
    }

    private void Base16UpperRadioItem_Click(object sender, RoutedEventArgs e)
    {
        Setting.Format = HashFormat.Base16Upper;
    }

    private void Base16LowerRadioItem_Click(object sender, RoutedEventArgs e)
    {
        Setting.Format = HashFormat.Base16Lower;
    }

    private void Base64RadioItem_Click(object sender, RoutedEventArgs e)
    {
        Setting.Format = HashFormat.Base64;
    }

    private bool ToHashFormatRadioItemIsChecked(HashFormat format, HashFormat targetFormat)
    {
        return format == targetFormat;
    }
}
