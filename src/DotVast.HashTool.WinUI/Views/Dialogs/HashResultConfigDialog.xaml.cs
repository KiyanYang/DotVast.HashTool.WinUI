// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.UI.Xaml.Controls;

namespace DotVast.HashTool.WinUI.Views.Dialogs;

public sealed partial class HashResultConfigDialog : ContentDialog
{
    public IList<HashOption> HashOptions { get; }

    public HashResultConfigDialog(IList<HashOption> hashOption)
    {
        HashOptions = hashOption;
        Resources.Add("HashFormats", Enum.GetValues<HashFormat>());
        Title = LocalizationPopup.HashResultConfig_Title;
        InitializeComponent();
    }
}
