using System.Text.Json.Serialization;

using DotVast.HashTool.WinUI.Core.Enums;

namespace DotVast.HashTool.WinUI.Enums;

[JsonConverter(typeof(JsonConverterFactoryForGenericEnumDerived))]
public sealed class HashTaskState : GenericEnum<string>
{
    /// <summary>
    /// 等待中.
    /// </summary>
    public static readonly HashTaskState Waiting = new(Localization.HashTaskState_Waiting);

    /// <summary>
    /// 计算中.
    /// </summary>
    public static readonly HashTaskState Working = new(Localization.HashTaskState_Working);

    /// <summary>
    /// 已完成.
    /// </summary>
    public static readonly HashTaskState Completed = new(Localization.HashTaskState_Completed);

    /// <summary>
    /// 任务取消.
    /// </summary>
    public static readonly HashTaskState Canceled = new(Localization.HashTaskState_Canceled);

    /// <summary>
    /// 任务中止(错误/意外).
    /// </summary>
    public static readonly HashTaskState Aborted = new(Localization.HashTaskState_Aborted);

    private HashTaskState(string name) : base(name) { }
}
