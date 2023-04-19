// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Models.Messages;

internal sealed record ValueUpdatedMessage<TOwner, TValue>(TOwner Owner, TValue Value);

internal sealed record PropertyUpdatedMessage<TOwner, TValue>(TOwner Owner, string PropertyName, TValue OldValue, TValue NewValue);
