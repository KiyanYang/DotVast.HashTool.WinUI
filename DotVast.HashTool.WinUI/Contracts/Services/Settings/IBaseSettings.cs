namespace DotVast.HashTool.WinUI.Contracts.Services.Settings;

public interface IBaseSettings
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
