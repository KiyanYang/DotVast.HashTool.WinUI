// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Messaging;

namespace DotVast.HashTool.WinUI.Models;

public sealed partial class HashTaskCheckable(HashTask hashTask, bool isChecked) : ObservableObject
{
    [ObservableProperty]
    public partial bool IsChecked { get; set; } = isChecked;

    partial void OnIsCheckedChanged(bool value) =>
        WeakReferenceMessenger.Default.SendV<HashTaskCheckable, bool>(new(this, value), EMT.HashTaskCheckable_IsChecked);

    public HashTask HashTask { get; } = hashTask;
}
