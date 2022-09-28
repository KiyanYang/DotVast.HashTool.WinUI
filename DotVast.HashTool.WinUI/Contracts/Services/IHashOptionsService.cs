using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IHashOptionsService
{
    List<HashOption> HashOptions
    {
        get;
    }

    Task InitializeAsync();

    Task SetHashOptionAsync(HashOption hashOption);
}
