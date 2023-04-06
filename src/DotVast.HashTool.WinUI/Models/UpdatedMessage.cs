namespace DotVast.HashTool.WinUI.Models.Messages;

internal record ValueUpdatedMessage<TOwner, TValue>(TOwner Owner, TValue Value);

internal record PropertyUpdatedMessage<TOwner, TValue>(TOwner Owner, string PropertyName, TValue OldValue, TValue NewValue);
