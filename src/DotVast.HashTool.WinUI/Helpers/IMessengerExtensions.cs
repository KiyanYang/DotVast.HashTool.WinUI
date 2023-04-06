using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models.Messages;

namespace DotVast.HashTool.WinUI.Helpers;

internal static class IMessengerExtensions
{
    internal static void RegisterV<TRecipient, TOwner, TValue>(this IMessenger messenger, TRecipient recipient, MessageToken token, Action<TRecipient, TOwner, TValue> action)
        where TRecipient : class
    {
#if !DEBUG
        messenger.Register<TRecipient, ValueUpdatedMessage<TOwner, TValue>, int>(recipient, (int)(object)token, (r, m) => action(r, m.Owner, m.Value));
#else
        messenger.Register<TRecipient, ValueUpdatedMessage<TOwner, TValue>, int>(recipient, (int)(object)token, (r, m) =>
        {
            var recipientTypeName = typeof(TRecipient).Name;
            var ownerTypeName = typeof(TOwner).Name;
            var valueTypeName = typeof(TValue).Name;
            Debug.WriteLine($"[{DateTime.Now}] {typeof(TRecipient).Name} : ValueUpdatedMessage<{ownerTypeName}, {valueTypeName}> : {Enum.GetName(token)}");
            Debug.WriteLine($"    Owner: {m.Owner}");
            Debug.WriteLine($"    Value: {m.Value}");
            action(r, m.Owner, m.Value);
        });
#endif
    }

    internal static void RegisterP<TRecipient, TOwner, TValue>(this IMessenger messenger, TRecipient recipient, MessageToken token, Action<TRecipient, TOwner, string, TValue, TValue> action)
        where TRecipient : class
    {
#if !DEBUG
        messenger.Register<TRecipient, PropertyUpdatedMessage<TOwner, TValue>, int>(recipient, (int)(object)token, (r, m) => action(r, m.Owner, m.PropertyName, m.OldValue, m.NewValue));
#else
        messenger.Register<TRecipient, PropertyUpdatedMessage<TOwner, TValue>, int>(recipient, (int)(object)token, (r, m) =>
        {
            var recipientTypeName = typeof(TRecipient).Name;
            var ownerTypeName = typeof(TOwner).Name;
            var valueTypeName = typeof(TValue).Name;
            Debug.WriteLine($"[{DateTime.Now}] {typeof(TRecipient).Name} : PropertyUpdatedMessage<{ownerTypeName}, {valueTypeName}> : {Enum.GetName(token)}");
            Debug.WriteLine($"    Owner: {m.Owner}");
            Debug.WriteLine($"    PropertyName: {m.PropertyName}");
            Debug.WriteLine($"    NewValue: {m.NewValue}");
            Debug.WriteLine($"    OldValue: {m.OldValue}");
            action(r, m.Owner, m.PropertyName, m.OldValue, m.NewValue);
        });
#endif
    }

    internal static ValueUpdatedMessage<TOwner, TValue> SendV<TOwner, TValue>(this IMessenger messenger, ValueUpdatedMessage<TOwner, TValue> message, MessageToken token)
    {
        return messenger.Send(message, (int)(object)token);
    }

    internal static PropertyUpdatedMessage<TOwner, TValue> SendP<TOwner, TValue>(this IMessenger messenger, PropertyUpdatedMessage<TOwner, TValue> message, MessageToken token)
    {
        return messenger.Send(message, (int)(object)token);
    }
}
