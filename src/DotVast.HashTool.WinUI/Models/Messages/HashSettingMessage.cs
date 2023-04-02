namespace DotVast.HashTool.WinUI.Models.Messages;

internal record HashSettingIsCheckedChangedMessage(HashSetting HashSetting, bool IsChecked);

internal record HashSettingIsEnabledForAppChangedMessage(HashSetting HashSetting, bool IsEnabledForApp);

internal record HashSettingIsEnabledForContextMenuChangedMessage(HashSetting HashSetting, bool IsEnabledForContextMenu);
