// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Models.Navigation;

internal enum HomeParameterKind
{
    EditHashTask,
}

// Don't use the "record" type. We need to control "equals" method,
// because we may perform equality judgment on parameter in INavigationService.NavigateTo.
/// <summary>
/// The parameter of navigation to Home.
/// </summary>
/// <param name="kind">The kind of parameter for navigation to Home.</param>
/// <param name="data">The data of parameter for navigation to Home.</param>
/// <remarks>
///     when Kind is <see cref="HomeParameterKind.EditHashTask"/>, the Data is <see cref="HashTask"/>.
/// </remarks>
internal sealed class HomeParameter(HomeParameterKind kind, object data)
{
    public HomeParameterKind Kind { get; } = kind;
    public object Data { get; } = data;
}
