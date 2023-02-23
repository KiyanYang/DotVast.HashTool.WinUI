using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HomeViewModel
{
    private const char FilesSeparator = '|';
    private const int MaxFilesCount = 100;

    private bool _isDelayCreateTask = false;

    [RelayCommand]
    private async Task PickAsync()
    {
        try
        {
            if (InputtingMode == HashTaskMode.File)
            {
                FileOpenPicker picker = new();
                picker.FileTypeFilter.Add("*");

                var hwnd = WinUIEx.HwndExtensions.GetActiveWindow();
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var result = await picker.PickMultipleFilesAsync();
                if (result.Count > 0)
                {
                    InputtingContent = string.Join(FilesSeparator, result.Take(MaxFilesCount).Select(r => r.Path));
                }
            }
            else if (InputtingMode == HashTaskMode.Folder)
            {
                FolderPicker picker = new();
                picker.FileTypeFilter.Add("*");

                var hwnd = WinUIEx.HwndExtensions.GetActiveWindow();
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

    [RelayCommand(CanExecute = nameof(CanCreateTask))]
    private void CreateTask()
    {
        var hashTask = CreateHashTask();
        if (StartingWhenCreateHashTask)
        {
            _ = hashTask.StartAsync();
        }
        _hashTaskService.HashTasks.Add(hashTask);
        _timer.Enabled = true;
    }

    private bool CanCreateTask()
    {
        if (_isDelayCreateTask)
        {
            return false;
        }
        if (HashOptions.All(h => h.IsChecked == false))
        {
            return false;
        }
        if (InputtingMode == HashTaskMode.File)
        {
            return FilesExists(InputtingContent);
        }
        if (InputtingMode == HashTaskMode.Folder)
        {
            return Directory.Exists(InputtingContent);
        }
        return true;
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
            InputtingContent = string.Join(FilesSeparator, items.Take(MaxFilesCount).Select(i => i.Path));
        }
    }

    [RelayCommand]
    private void ShowResult(HashTask hashTask)
    {
        _navigationService.NavigateTo(Constants.PageKeys.ResultsPage, parameter: hashTask);
    }

    #region Helper

    private HashTask CreateHashTask()
    {
        return new()
        {
            DateTime = DateTime.Now,
            Mode = InputtingMode,
            Content = InputtingMode switch
            {
                var m when m == HashTaskMode.Folder => PathTrim(InputtingContent),
                var m when m == HashTaskMode.File => string.Join(FilesSeparator, InputtingContent.Split(FilesSeparator).Select(i => PathTrim(i))),
                _ => InputtingContent,
            },
            Encoding = InputtingMode == HashTaskMode.Text ? InputtingTextEncoding.Encoding.Value : null,
            SelectedHashs = HashOptions.Where(i => i.IsChecked).Select(i => i.Hash).ToArray(),
            State = HashTaskState.Waiting,
        };
    }

    /// <summary>
    /// 修剪路径, 去除其前后空白和引号(").
    /// </summary>
    /// <param name="path">路径.</param>
    /// <returns></returns>
    private static string PathTrim(string path)
    {
        return path.Trim().Trim('"');
    }

    /// <summary>
    /// 验证文件是否存在.
    /// </summary>
    /// <returns>文件均存在为 <see langword="true"/>, 否则为 <see langword="false"/>.</returns>
    private static bool FilesExists(string paths)
    {
        return paths.Split(FilesSeparator)
                    .Select(p => PathTrim(p))
                    .All(p => File.Exists(p));
    }

    #endregion Helper
}
