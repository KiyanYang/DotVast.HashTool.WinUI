// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Helpers;

public static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "软件已启动, 用时: {Elapsed}.")]
    public static partial void AppLaunchedElapsedTime(this ILogger logger, TimeSpan elapsed);

    [LoggerMessage(Level = LogLevel.Information, Message = "AppWindow '{Location}' Destroying.")]
    public static partial void AppWindowDestroying(this ILogger logger, string? location);

    [LoggerMessage(Level = LogLevel.Information, Message = "Window '{Location}' Closed.")]
    public static partial void WindowClosed(this ILogger logger, string? location);

    #region Navigation

    [LoggerMessage(Level = LogLevel.Information, Message = "导航进入 {Location}, 参数为 {Args}.")]
    public static partial void NavigatedTo(this ILogger logger, string? location, object? args);

    [LoggerMessage(Level = LogLevel.Information, Message = "导航返回 {Location}.")]
    public static partial void GoBackTo(this ILogger logger, string? location);

    #endregion Navigation

    #region Activation

    [LoggerMessage(Level = LogLevel.Information, Message = "处理 Microsoft.Windows.AppLifecycle.AppActivationArguments 激活, 激活类型: {Kind}.")]
    public static partial void HandleLaunchActivation(this ILogger logger, Microsoft.Windows.AppLifecycle.ExtendedActivationKind kind);

    [LoggerMessage(Level = LogLevel.Information, Message = "处理 Microsoft.UI.Xaml.LaunchActivatedEventArgs 默认激活, 激活参数: {Args}.")]
    public static partial void HandleDefaultLaunchActivation(this ILogger logger, object args);

    [LoggerMessage(Level = LogLevel.Information, Message = "已启动 {ActivationType} 激活, 激活类型: {Kind}, 激活参数: {Args}.")]
    public static partial void LaunchActivated(this ILogger logger, Type activationType, Enum kind, object args);

    #endregion Activation

    [LoggerMessage(Level = LogLevel.Error, Message = "软件出现异常:")]
    public static partial void AppException(this ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "启动激活时出现异常:")]
    public static partial void LaunchActivatedException(this ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Critical, Message = "软件出现未处理的异常:")]
    public static partial void AppUnhandledException(this ILogger logger, Exception exception);
}
