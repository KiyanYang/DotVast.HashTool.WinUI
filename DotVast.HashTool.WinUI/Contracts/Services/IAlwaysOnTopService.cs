namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IAlwaysOnTopService
{
    bool IsAlwaysOnTop
    {
        get;
    }

    Task InitializeAsync();

    Task SetIsAlwaysOnTopAsync(bool isAlwaysOnTop);

    Task SetRequestedIsAlwaysOnTopAsync();
}
