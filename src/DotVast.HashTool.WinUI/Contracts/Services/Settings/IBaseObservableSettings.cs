using System.ComponentModel;

namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IBaseObservableSettings : INotifyPropertyChanged
{
    /// <summary>
    /// InitializeAsync in <see cref="IActivationService.ActivateAsync(object)"/>.
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();

    /// <summary>
    /// StartupAsync <see cref="IActivationService.ActivateAsync(object)"/>.
    /// </summary>
    /// <returns></returns>
    Task StartupAsync();
}
