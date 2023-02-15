using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.Helpers;

public static partial class Log
{
    [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "软件已启动, 用时: {Elapsed} ms.")]
    public static partial void AppLaunchedElapsedTime(this ILogger logger, long elapsed);

    [LoggerMessage(EventId = 100, Level = LogLevel.Information, Message = "导航进入 {Location}, 参数为 {Arg}.")]
    public static partial void NavigatedTo(this ILogger logger, string? location, object? arg);

    [LoggerMessage(EventId = 101, Level = LogLevel.Information, Message = "导航返回 {Location}.")]
    public static partial void GoBackTo(this ILogger logger, string? location);
}
