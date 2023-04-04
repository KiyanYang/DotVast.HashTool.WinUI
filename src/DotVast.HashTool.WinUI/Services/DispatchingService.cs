using Microsoft.UI.Dispatching;

namespace DotVast.HashTool.WinUI.Services;

internal class DispatchingService : IDispatchingService
{
    private DispatcherQueue? _dispatcherQueue;

    public void Initialize(DispatcherQueue dispatcherQueue)
    {
        _dispatcherQueue = dispatcherQueue;
    }

    public bool TryEnqueue(Action callback)
    {
        return TryEnqueue(DispatcherQueuePriority.Normal, callback);
    }

    public bool TryEnqueue(DispatcherQueuePriority priority, Action callback)
    {
        if (_dispatcherQueue is null)
        {
            throw new InvalidOperationException();
        }

        if (_dispatcherQueue.HasThreadAccess)
        {
            callback();
            return true;
        }

        return _dispatcherQueue.TryEnqueue(priority, () => callback());
    }
}
