namespace DotVast.HashTool.WinUI.Models.Navigation;

internal enum HomeParameterKind
{
    EditHashTask,
}

// Don't use the "record" type. We need to control "equals" method,
// because we may perform equality judgment on parameter in INavigationService.NavigateTo.
internal class HomeParameter
{
    /// <summary>
    /// The parameter of navigation to Home.
    /// </summary>
    /// <param name="Kind">The kind of parameter for navigation to Home.</param>
    /// <param name="Data">The data of parameter for navigation to Home.</param>
    /// <remarks>
    ///     when Kind is <see cref="HomeParameterKind.EditHashTask"/>, the Data is <see cref="HashTask"/>.
    /// </remarks>
    public HomeParameter(HomeParameterKind Kind, object Data)
    {
        this.Kind = Kind;
        this.Data = Data;
    }

    public HomeParameterKind Kind { get; }
    public object Data { get; }
}
