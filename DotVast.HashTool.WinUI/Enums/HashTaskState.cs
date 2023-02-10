using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Core.Enums;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonConverterFactoryForGenericEnumDerived))]
public sealed class HashTaskState : GenericEnum<string>
{
    /// <summary>
    /// 等待中(从未进行过计算).
    /// </summary>
    public static readonly HashTaskState Waiting = new(Localization.HashTaskState_Waiting);

    /// <summary>
    /// 计算中(正在计算, 未暂停).
    /// </summary>
    public static readonly HashTaskState Working = new(Localization.HashTaskState_Working);

    public static readonly HashTaskState Paused = new("已暂停");

    /// <summary>
    /// 已完成(正常完成计算).
    /// </summary>
    public static readonly HashTaskState Completed = new(Localization.HashTaskState_Completed);

    /// <summary>
    /// 已取消(用户主动).
    /// </summary>
    public static readonly HashTaskState Canceled = new(Localization.HashTaskState_Canceled);

    /// <summary>
    /// 已中止(错误/意外).
    /// </summary>
    public static readonly HashTaskState Aborted = new(Localization.HashTaskState_Aborted);

    private HashTaskState(string name) : base(name) { }
}
