using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HomeViewModel
{
    [RelayCommand]
    private async Task PickAsync()
    {
        try
        {
            if (InputtingMode == HashTaskMode.File)
            {
                FileOpenPicker picker = new();
                picker.FileTypeFilter.Add("*");

                var hwnd = HwndExtensions.GetActiveWindow();
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var result = await picker.PickMultipleFilesAsync();
                if (result != null)
                {
                    InputtingContent = string.Join('|', result.Select(r => r.Path));
                }
            }
            else if (InputtingMode == HashTaskMode.Folder)
            {
                FolderPicker picker = new();
                picker.FileTypeFilter.Add("*");

                var hwnd = HwndExtensions.GetActiveWindow();
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var result = await picker.PickSingleFolderAsync();
                if (result != null)
                {
                    InputtingContent = result.Path;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("选取{Mode}时出现未预料的异常\n{Exception}", InputtingMode, ex);
        }
    }

    public bool CanExecuteStart => _computeHashService.Status == ComputeHashStatus.Free;

    [RelayCommand(CanExecute = nameof(CanExecuteStart))]
    private async Task StartTaskAsync()
    {
        _mres.Set();
        await ComputeHashAsync();
    }

    public bool CanExecuteReset => _computeHashService.Status != ComputeHashStatus.Free;

    [RelayCommand(CanExecute = nameof(CanExecuteReset))]
    private void ResetTask()
    {
        if (_mres.IsSet)
        {
            ResetButton.Content = Localization.Home_Button_Resume;
            _mres.Reset();
        }
        else
        {
            ResetButton.Content = Localization.Home_Button_Pause;
            _mres.Set();
        }
    }

    public bool CanExecuteCancel => _computeHashService.Status != ComputeHashStatus.Free;

    [RelayCommand(CanExecute = nameof(CanExecuteCancel))]
    private void CancelTask()
    {
        _cts!.Cancel();
        ResetButton.Content = Localization.Home_Button_Pause;
        _mres.Set();
    }

    /// <summary>
    /// 保存哈希选项.
    /// </summary>
    [RelayCommand]
    private async Task SaveHashOptionAsync() =>
        await _preferencesSettingsService.SaveHashOptionsAsync();

    public async Task SetHashTaskContenFromDrag(DataPackageView view)
    {
        if (!view.Contains(StandardDataFormats.StorageItems))
        {
            return;
        }

        var items = await view.GetStorageItemsAsync();
        if (items.Count > 0)
        {
            InputtingContent = string.Join('|', items.Select(i => i.Path));
        }
    }

    #region Helper

    private int _hashTaskId = 1;

    private HashTask CreateHashTask()
    {
        HashTask hashTask = new()
        {
            Id = _hashTaskId,
            DateTime = DateTime.Now,
            Mode = InputtingMode,
            Content = InputtingMode == HashTaskMode.Text
                ? InputtingContent
                : string.Join('|', InputtingContent.Split('|').Select(i => i.Trim().Trim('"'))),
            Encoding = InputtingTextEncoding.Encoding,
            SelectedHashs = HashOptions.Where(i => i.IsChecked).Select(i => i.Hash).ToList(),
            State = HashTaskState.Waiting,
        };
        _hashTaskId++;
        return hashTask;
    }

    private async Task ComputeHashAsync()
    {
        _cts = new();

        var hashTask = CreateHashTask();
        CurrentHashTask = hashTask;
        _hashTaskService.HashTasks.Add(hashTask);

        try
        {
            switch (hashTask.Mode)
            {
                case var m when m == HashTaskMode.Text:
                    await _computeHashService.HashTextAsync(hashTask, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.File:
                    // 仅验证第一个文件是否存在, 以便尽快进入实际计算
                    if (File.Exists(hashTask.Content.Split('|')[0]) == false)
                    {
                        hashTask.State = HashTaskState.Aborted;
                        await _dialogService.ShowInfoDialogAsync(
                            Localization.Dialog_HashTaskAborted_Title,
                            Localization.Dialog_HashTaskAborted_FileNotExists,
                            Localization.Dialog_HashTaskAborted_OK);
                        return;
                    }
                    await _computeHashService.HashFileAsync(hashTask, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.Folder:
                    if (Directory.Exists(hashTask.Content) == false)
                    {
                        hashTask.State = HashTaskState.Aborted;
                        await _dialogService.ShowInfoDialogAsync(
                            Localization.Dialog_HashTaskAborted_Title,
                            Localization.Dialog_HashTaskAborted_FolderNotExists,
                            Localization.Dialog_HashTaskAborted_OK);
                        return;
                    }
                    await _computeHashService.HashFolderAsync(hashTask, _mres, _cts.Token);
                    break;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            await _dialogService.ShowInfoDialogAsync(
                Localization.Dialog_HashTaskAborted_Title,
                Localization.Dialog_HashTaskAborted_UnauthorizedAccess,
                Localization.Dialog_HashTaskAborted_OK);
            _logger.LogWarning("计算哈希时出现“未授权访问”异常, 模式: {Mode}, 内容: {Content}\n{Exception}", hashTask.Mode, hashTask.Content, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("计算哈希时出现未预料的异常, 哈希任务: {HashTask:j}\n{Exception}", hashTask, ex);
        }
        finally
        {
            _cts.Dispose();
        }
    }

    #endregion Helper
}
