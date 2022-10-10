using System.ComponentModel;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IBaseObservableSettings : INotifyPropertyChanged
{
    /// <summary>
    /// Execute tasks before activation.
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();

    /// <summary>
    /// Execute tasks after activation.
    /// </summary>
    /// <returns></returns>
    Task StartupAsync();
}
