using CommunityToolkit.Labs.WinUI;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface INotificationService
{
    /// <summary>
    /// 初始化 <see cref="StackedNotificationsBehavior"/> 实例.
    /// </summary>
    /// <param name="navigationView"></param>
    void Initialize(StackedNotificationsBehavior notificationQueue);

    /// <summary>
    /// 显示通知.
    /// </summary>
    /// <param name="notification">通知.</param>
    void Show(Notification notification);
}
