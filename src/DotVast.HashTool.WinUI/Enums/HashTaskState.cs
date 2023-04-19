// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashTaskState
{
    /// <summary>
    /// 等待中(从未进行过计算).
    /// </summary>
    Waiting,

    /// <summary>
    /// 计算中(正在计算, 未暂停).
    /// </summary>
    Working,

    /// <summary>
    /// 已暂停(正常计算期间暂停).
    /// </summary>
    Paused,

    /// <summary>
    /// 已完成(正常完成计算).
    /// </summary>
    Completed,

    /// <summary>
    /// 已取消(用户主动).
    /// </summary>
    Canceled,

    /// <summary>
    /// 已中止(错误/意外).
    /// </summary>
    Aborted,
}

public static class HashTaskStateExtensions
{
    public static string ToDisplay(this HashTaskState hashTaskState)
    {
        return hashTaskState switch
        {
            HashTaskState.Waiting => LocalizationEnum.HashTaskState_Waiting,
            HashTaskState.Working => LocalizationEnum.HashTaskState_Working,
            HashTaskState.Paused => LocalizationEnum.HashTaskState_Paused,
            HashTaskState.Completed => LocalizationEnum.HashTaskState_Completed,
            HashTaskState.Canceled => LocalizationEnum.HashTaskState_Canceled,
            HashTaskState.Aborted => LocalizationEnum.HashTaskState_Aborted,
            _ => throw new ArgumentOutOfRangeException(nameof(hashTaskState)),
        };
    }
}
