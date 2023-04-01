namespace DotVast.HashTool.WinUI.Models.Messages;

internal record HashOptionIsCheckedChangedMessage(HashOption HashOption, bool IsChecked);

internal record HashOptionIsEnabledChangedMessage(HashOption HashOption, bool IsEnabled);
