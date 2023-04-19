// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Messaging;

namespace DotVast.HashTool.WinUI.ViewModels;

public abstract partial class SimpleObservableRecipient : ObservableObject
{
    protected SimpleObservableRecipient() : this(WeakReferenceMessenger.Default) { }

    protected SimpleObservableRecipient(IMessenger messenger)
    {
        Messenger = messenger;
    }

    protected IMessenger Messenger { get; }

    [ObservableProperty]
    private bool _isActive;

    partial void OnIsActiveChanged(bool value)
    {
        if (value)
        {
            OnActivated();
        }
        else
        {
            OnDeactivated();
        }
    }

    protected virtual void OnActivated()
    {
        throw new NotImplementedException();
    }

    protected virtual void OnDeactivated()
    {
        Messenger.UnregisterAll(this);
    }
}
