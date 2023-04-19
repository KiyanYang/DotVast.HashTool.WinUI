// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

using CommunityToolkit.Labs.WinUI;

namespace DotVast.HashTool.WinUI.Services;

public sealed class NotificationService : INotificationService
{
    private StackedNotificationsBehavior? _notificationQueue { get; set; }

    [MemberNotNull(nameof(_notificationQueue))]
    public void Initialize(StackedNotificationsBehavior notificationQueue)
    {
        _notificationQueue = notificationQueue;
    }

    public void Show(Notification notification)
    {
        _notificationQueue?.Show(notification);
    }
}
