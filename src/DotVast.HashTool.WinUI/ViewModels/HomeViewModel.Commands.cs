// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Microsoft.Extensions.Logging;

using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public partial class HomeViewModel
{
    private const char FilesSeparator = '|';
    private const int MaxFilesCount = 100;
    private const int MillisecondsDelayForCreateTask = 750;

    [RelayCommand]
    private async Task PickAsync()
    {
        try
        {
            if (InputtingMode == HashTaskMode.Files)
            {
                FileOpenPicker picker = new();
                picker.FileTypeFilter.Add("*");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
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

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
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
            _logger.LogError("选取{Mode}时出现异常\n{Exception}", InputtingMode, ex);
            _notificationService.Show(new()
            {
                Title = LocalizationPopup.Exception_Title_Exception,
                Message = string.Format(LocalizationPopup.Exception_Message_ExceptionOccurredWhenPick_F, InputtingMode.ToDisplay().ToLower()),
                Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error,
                Duration = TimeSpan.FromSeconds(3),
            });
        }
    }

    [RelayCommand(CanExecute = nameof(CanCreateTask))]
    private async Task CreateTaskAsync()
    {
        var hashTask = new HashTask()
        {
            Mode = InputtingMode,
            Content = InputtingMode switch
            {
                HashTaskMode.Files => string.Join(FilesSeparator, InputtingContent.Split(FilesSeparator).Select(PathTrim)),
                HashTaskMode.Folder => PathTrim(InputtingContent),
                _ => throw new InvalidOperationException(),
            },
            HashOptions = HashSettings.Where(i => i.IsChecked).Select(i => new HashOption { Kind = i.Kind, Format = i.Format }).ToArray(),
            State = HashTaskState.Waiting,
        };
        if (StartingWhenCreateHashTask)
        {
            _ = hashTask.StartAsync();
        }
        _hashTaskService.HashTasks.Add(hashTask);
        await Task.Delay(MillisecondsDelayForCreateTask);
    }

    private bool CanCreateTask()
    {
        if (!HashSettings.Any(h => h.IsChecked))
        {
            return false;
        }
        if (InputtingMode == HashTaskMode.Files)
        {
            return FilesExists(InputtingContent);
        }
        if (InputtingMode == HashTaskMode.Folder)
        {
            return Directory.Exists(InputtingContent);
        }
        return true;
    }

    public void SetHashTaskContenFromPaths(IEnumerable<string> paths)
    {
        InputtingContent = string.Join(FilesSeparator, paths.Take(MaxFilesCount));
    }

    [RelayCommand]
    private void ShowResult(HashTask hashTask)
    {
        _navigationService.NavigateTo(PageKey.ResultsPage, parameter: hashTask);
    }

    #region Helper

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
        return paths.Split(FilesSeparator).Select(PathTrim).All(File.Exists);
    }

    #endregion Helper
}
