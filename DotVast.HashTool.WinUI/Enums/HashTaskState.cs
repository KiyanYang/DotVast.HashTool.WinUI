namespace DotVast.HashTool.WinUI.Enums;

public sealed class HashTaskState : GenericEnum<string>
{
    /// <summary>
    /// 等待中.
    /// </summary>
    public static HashTaskState Waiting { get; } = new(Localization.HashTaskState_Waiting);

    /// <summary>
    /// 计算中.
    /// </summary>
    public static HashTaskState Working { get; } = new(Localization.HashTaskState_Working);

    /// <summary>
    /// 已完成.
    /// </summary>
    public static HashTaskState Completed { get; } = new(Localization.HashTaskState_Completed);

    /// <summary>
    /// 任务取消.
    /// </summary>
    public static HashTaskState Canceled { get; } = new(Localization.HashTaskState_Canceled);

    /// <summary>
    /// 任务中止(错误/意外).
    /// </summary>
    public static HashTaskState Aborted { get; } = new(Localization.HashTaskState_Aborted);

    private HashTaskState(string name) : base(name) { }
}
