using Microsoft.UI.Dispatching;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IDispatchingService
{
    void Initialize(DispatcherQueue dispatcherQueue);

    bool TryEnqueue(Action callback);

    bool TryEnqueue(DispatcherQueuePriority priority, Action callback);
}
