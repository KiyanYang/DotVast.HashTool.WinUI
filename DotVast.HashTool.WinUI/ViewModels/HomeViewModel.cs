using System.Diagnostics;
using System.Text;

using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.WinUI;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Controls;
using DotVast.HashTool.WinUI.Models.Messages;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient
{
    private readonly ILogger<HomeViewModel> _logger;
    private readonly IComputeHashService _computeHashService;
    private readonly IDialogService _dialogService;
    private readonly IHashOptionsService _hashOptionsService;
    private readonly IHashTaskService _hashTaskService;
    private readonly ManualResetEventSlim _mres = new(true);
    private CancellationTokenSource? _cts;

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IComputeHashService computeHashService,
        IDialogService dialogService,
        IHashOptionsService hashOptionsService,
        IHashTaskService hashTaskService)
    {
        _logger = logger;
        _computeHashService = computeHashService;
        _dialogService = dialogService;
        _hashOptionsService = hashOptionsService;
        _hashTaskService = hashTaskService;

        _computeHashService.AtomProgressChanged += (sender, e) => AtomProgressBar.Val = e;
        _computeHashService.TaskProgressChanged += (sender, e) => (TaskProgressBar.Val, TaskProgressBar.Max) = e;

        IsActive = true;
    }

    ~HomeViewModel()
    {
        _mres.Dispose();
        _cts?.Dispose();
    }

    #region Public Properties

    /// <summary>
    /// 当前界面显示的哈希任务.
    /// </summary>
    public HashTask CurrentHashTask { get; } = new() { Encoding = Encoding.UTF8 };

    /// <summary>
    /// 哈希任务模式. 文件, 文件夹, 文本.
    /// </summary>
    public HashTaskMode[] HashTaskModes { get; } = new[] { HashTaskMode.File, HashTaskMode.Folder, HashTaskMode.Text };

    /// <summary>
    /// 当前流计算的进度.
    /// </summary>
    public ProgressBarModel AtomProgressBar { get; } = new() { Min = 0, Max = 1, Val = 0 };

    /// <summary>
    /// 当前任务的进度.
    /// </summary>
    public ProgressBarModel TaskProgressBar { get; } = new() { Min = 0, Max = 1, Val = 0 };

    /// <summary>
    /// “开始”按钮.
    /// </summary>
    public ButtonModel StartButton { get; } = new() { IsEnabled = true, Content = Localization.Home_Button_Start };

    /// <summary>
    /// “暂停”和“继续”按钮.
    /// </summary>
    public ButtonModel ResetButton { get; } = new() { IsEnabled = false, Content = Localization.Home_Button_Pause };

    /// <summary>
    /// “取消”按钮.
    /// </summary>
    public ButtonModel CancelButton { get; } = new() { IsEnabled = false, Content = Localization.Home_Button_Cancel };

    /// <summary>
    /// Hash 选项.
    /// </summary>
    public IEnumerable<HashOption> HashOptions => _hashOptionsService.HashOptions.Where(i => i.IsEnabled);

    public record TextEncoding(string Name, Encoding Encoding);

    public TextEncoding UTF8 { get; } = new("UTF-8", Encoding.UTF8);

    private IList<TextEncoding>? _textEncodings;

    /// <summary>
    /// 文本编码.
    /// </summary>
    public IList<TextEncoding> TextEncodings
    {
        get
        {
            if (_textEncodings == null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _textEncodings = Encoding.GetEncodings()
                    .Select(e => new TextEncoding(e.Name.ToUpper(), e.GetEncoding()))
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            return _textEncodings;
        }
    }

    #region Verifying Hash

    private string _verifyingHash1 = string.Empty;
    private string _verifyingHash2 = string.Empty;

    public string VerifyingHash1
    {
        get => _verifyingHash1;
        set
        {
            SetProperty(ref _verifyingHash1, value.Trim());
            OnPropertyChanged(nameof(VerifyingResult));
        }
    }

    public string VerifyingHash2
    {
        get => _verifyingHash2;
        set
        {
            SetProperty(ref _verifyingHash2, value.Trim());
            OnPropertyChanged(nameof(VerifyingResult));
        }
    }

    public string? VerifyingResult
    {
        get
        {
            if (VerifyingHash1 == string.Empty || VerifyingHash2 == string.Empty)
            {
                return null;
            }

            if (string.Equals(VerifyingHash1, VerifyingHash2, StringComparison.Ordinal))
            {
                return "=";
            }

            // Quick Xor, Base64, 必须完全相等
            if (!VerifyingHash1.EndsWith('=')
                && string.Equals(VerifyingHash1, VerifyingHash2, StringComparison.OrdinalIgnoreCase))
            {
                return "≈";
            }

            return "≠";
        }
    }

    #endregion Verifying Hash

    public TeachingTipModel TipMessage { get; } = new();

    #endregion Public Properties

    #region Commands

    [RelayCommand]
    public async Task Pick()
    {
        if (CurrentHashTask.Mode == HashTaskMode.File)
        {
            FileOpenPicker picker = new();
            picker.FileTypeFilter.Add("*");

            var hwnd = HwndExtensions.GetActiveWindow();
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var result = await picker.PickSingleFileAsync();
            if (result != null)
            {
                CurrentHashTask.Content = result.Path;
            }
        }
        else if (CurrentHashTask.Mode == HashTaskMode.Folder)
        {
            FolderPicker picker = new();
            picker.FileTypeFilter.Add("*");

            var hwnd = HwndExtensions.GetActiveWindow();
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var result = await picker.PickSingleFolderAsync();
            if (result != null)
            {
                CurrentHashTask.Content = result.Path;
            }
        }
    }

    [RelayCommand]
    private void StartTask()
    {
        if (_computeHashService.Status == ComputeHashStatus.Free)
        {
            _mres.Set();
            ComputeHash();
        }
    }

    [RelayCommand]
    private void ResetTask()
    {
        if (_computeHashService.Status == ComputeHashStatus.Free)
        {
            return;
        }
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

    [RelayCommand]
    private void CancelTask()
    {
        if (_computeHashService.Status == ComputeHashStatus.Free)
        {
            return;
        }

        _cts!.Cancel();
        ResetButton.Content = Localization.Home_Button_Pause;
        _mres.Set();
    }

    [RelayCommand]
    private async Task SetHashOptionAsync(HashOption hashOption) =>
        await _hashOptionsService.SetHashOptionAsync(hashOption);

    public async Task SetHashTaskContenFromDrag(DataPackageView view)
    {
        if (!view.Contains(StandardDataFormats.StorageItems))
        {
            return;
        }

        var items = await view.GetStorageItemsAsync();
        if (items.Count > 0)
        {
            CurrentHashTask.Content = items[0].Path;
        }
    }

    #endregion Commands

    #region Messenger

    protected override void OnActivated()
    {
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        Messenger.Register<HomeViewModel, ComputeHashStatueChangedMessage>(this, async (r, m) =>
        {
            await dispatcherQueue.EnqueueAsync(() => SetButtonsStatus(m.Value));
        });
        Messenger.Register<HomeViewModel, FileNotFoundInHashFolderMessage>(this, async (r, m) =>
        {
            await dispatcherQueue.EnqueueAsync(() => ShowTipMessage(
                    Localization.Tip_FileSkipped_Title,
                    string.Format(Localization.Tip_FileSkipped_FileNotFound, m.Value)));
        });

        // PropertyChangedMessage[HashOption.IsEnabled]
        Messenger.Register<HomeViewModel, PropertyChangedMessage<bool>>(this, (r, m) =>
        {
            if (m.Sender is HashOption hashOption && m.PropertyName == nameof(HashOption.IsEnabled))
            {
                Debug.WriteLine($"---------------- {DateTime.Now} -- HomeViewModel.Messenger.PropertyChangedMessage[HashOption.IsEnabled]");
                Debug.WriteLine($"Hash.Name: {hashOption.Hash.Name}");
                Debug.WriteLine($"IsEnabled:{hashOption.IsEnabled}");

                OnPropertyChanged(nameof(HashOptions));
            }
        });
    }

    #endregion Messenger

    #region Helpers

    public async void ComputeHash()
    {
        _cts?.Dispose();
        _cts = new();

        var hashTask = SealHashTask();
        _hashTaskService.HashTasks.Add(hashTask);

        try
        {
            switch (hashTask.Mode)
            {
                case var m when m == HashTaskMode.Text:
                    await _computeHashService.HashText(hashTask, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.File:
                    if (File.Exists(hashTask.Content) == false)
                    {
                        hashTask.State = HashTaskState.Aborted;
                        await _dialogService.ShowInfoDialogAsync(
                            Localization.Dialog_HashTaskAborted_Title,
                            Localization.Dialog_HashTaskAborted_FileNotExists,
                            Localization.Dialog_HashTaskAborted_OK);
                        return;
                    }
                    await _computeHashService.HashFile(hashTask, _mres, _cts.Token);
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
                    await _computeHashService.HashFolder(hashTask, _mres, _cts.Token);
                    break;
            }
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning("计算哈希时出现“文件未找到”异常, 模式: {Mode}, 内容: {Content}\n{Exception}", hashTask.Mode, hashTask.Content, ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogWarning("计算哈希时出现“文件夹未找到”异常, 模式: {Mode}, 内容: {Content}\n{Exception}", hashTask.Mode, hashTask.Content, ex);
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
    }

    private int _hashTaskId = 0;
    private HashTask SealHashTask()
    {
        HashTask hashTask = new()
        {
            Id = _hashTaskId,
            DateTime = DateTime.Now,
            Mode = CurrentHashTask.Mode,
            Content = CurrentHashTask.Content,
            Encoding = CurrentHashTask.Encoding,
            SelectedHashs = HashOptions!.Where(i => i.IsChecked).Select(i => i.Hash).ToList(),
            State = HashTaskState.Waiting,
        };
        _hashTaskId++;
        return hashTask;
    }

    private void SetButtonsStatus(ComputeHashStatus status)
    {
        switch (status)
        {
            case ComputeHashStatus.Free:
                StartButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
                CancelButton.IsEnabled = false;
                ResetButton.Content = Localization.Home_Button_Pause;
                AtomProgressBar.ShowPaused = false;
                TaskProgressBar.ShowPaused = false;
                break;
            case ComputeHashStatus.Busy:
                StartButton.IsEnabled = false;
                ResetButton.IsEnabled = true;
                CancelButton.IsEnabled = true;
                ResetButton.Content = Localization.Home_Button_Pause;
                AtomProgressBar.ShowPaused = false;
                TaskProgressBar.ShowPaused = false;
                break;
            case ComputeHashStatus.Pasue:
                StartButton.IsEnabled = false;
                ResetButton.IsEnabled = true;
                CancelButton.IsEnabled = true;
                ResetButton.Content = Localization.Home_Button_Resume;
                AtomProgressBar.ShowPaused = true;
                TaskProgressBar.ShowPaused = true;
                break;
            default:
                break;
        }
    }

    private void ShowTipMessage(string title, string subTitle)
    {
        TipMessage.Title = title;
        TipMessage.Subtitle = subTitle;
        TipMessage.IsOpen = true;
    }

    #endregion Helpers
}
