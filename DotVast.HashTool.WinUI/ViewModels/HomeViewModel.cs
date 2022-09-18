using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Controls;
using DotVast.HashTool.WinUI.Services.Hash;

using Microsoft.Extensions.Logging;

using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient
{
    private readonly ILogger<HomeViewModel> _logger;
    private readonly IHashTaskService _hashTaskService;
    private readonly IComputeHashService _computeHashService;
    private readonly ManualResetEventSlim _mres = new(true);
    private CancellationTokenSource? _cts;

    private const string Uid_ButtonStart = "Home_Button_Start";
    private const string Uid_ButtonPause = "Home_Button_Pause";
    private const string Uid_ButtonResume = "Home_Button_Resume";
    private const string Uid_ButtonCancel = "Home_Button_Cancel";

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IHashTaskService hashTaskService,
        IComputeHashService computeHashService)
    {
        _logger = logger;
        _hashTaskService = hashTaskService;
        _computeHashService = computeHashService;
        _computeHashService.AtomProgress.ProgressChanged += (sender, e) => AtomProgressBar.Val = e;
        _computeHashService.TaskProgress.ProgressChanged += (sender, e) =>
        {
            TaskProgressBar.Val = e.Val;
            TaskProgressBar.Max = e.Max;
        };
        InitHashNames();
    }

    ~HomeViewModel()
    {
        _mres.Dispose();
        _cts?.Dispose();
    }

    #region public Properties

    /// <summary>
    /// 当前界面显示的哈希任务.
    /// </summary>
    public HashTask CurrentHashTask { get; } = new();

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
    public ButtonModel StartButton { get; } = new() { IsEnabled = true, Uid = Uid_ButtonStart };

    /// <summary>
    /// “暂停”和“继续”按钮.
    /// </summary>
    public ButtonModel ResetButton { get; } = new() { IsEnabled = false, Uid = Uid_ButtonPause };

    /// <summary>
    /// “取消”按钮.
    /// </summary>
    public ButtonModel CancelButton { get; } = new() { IsEnabled = false, Uid = Uid_ButtonCancel };

    /// <summary>
    /// Hash 选项.
    /// </summary>
    public List<HashOption>? HashOptions
    {
        get; private set;
    }

    #endregion public Properties

    #region InitMethods

    private void InitHashNames()
    {
        HashOptions = new()
        {
            // CRC
            new(false, Hash.CRC32),

            // MD
            //new(false,Hash.MD4),
            new(true, Hash.MD5),

            // SHA
            new(false, Hash.SHA1),
            //new(false,Hash.SHA224),
            new(true, Hash.SHA256),
            new(false, Hash.SHA384),
            new(false, Hash.SHA512),
            //new(false,Hash.SHA3_224),
            //new(false,Hash.SHA3_256),
            //new(false,Hash.SHA3_384),
            //new(false,Hash.SHA3_512),

            // Blake2
            //new(false,Hash.Blake2B_160),
            //new(false,Hash.Blake2B_256),
            //new(false,Hash.Blake2B_384),
            //new(false,Hash.Blake2B_512),
            //new(false,Hash.Blake2S_128),
            //new(false,Hash.Blake2S_160),
            //new(false,Hash.Blake2S_224),
            //new(false,Hash.Blake2S_256),

            // Keccak
            //new(false,Hash.Keccak_224),
            //new(false,Hash.Keccak_256),
            //new(false,Hash.Keccak_288),
            //new(false,Hash.Keccak_384),
            //new(false,Hash.Keccak_512),

            // QuickXor
            new(false, Hash.QuickXor),
        };
    }

    #endregion InitMethods

    #region Command

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
        if (_computeHashService.IsFree)
        {
            SetButtonsIsEnabled(false, true, true);
            _mres.Set();
            ComputeHash();
        }
    }

    [RelayCommand]
    private void ResetTask()
    {
        if (_computeHashService.IsFree)
        {
            return;
        }
        if (_mres.IsSet)
        {
            ResetButton.Uid = Uid_ButtonResume;
            _mres.Reset();
        }
        else
        {
            ResetButton.Uid = Uid_ButtonPause;
            _mres.Set();
        }
    }

    [RelayCommand]
    private void CancelTask()
    {
        if (_computeHashService.IsFree)
        {
            return;
        }
        SetButtonsIsEnabled(true, false, false);

        _cts!.Cancel();
        ResetButton.Uid = Uid_ButtonPause;
        _mres.Set();
    }

    #endregion Command

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
                        return;
                    }
                    await _computeHashService.HashFile(hashTask, _mres, _cts.Token);
                    break;

                case var m when m == HashTaskMode.Folder:
                    if (Directory.Exists(hashTask.Content) == false)
                    {
                        hashTask.State = HashTaskState.Aborted;
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
        catch (Exception ex)
        {
            _logger.LogError("计算哈希时出现未预料的异常, 哈希任务: {HashTask:j}\n{Exception}", hashTask, ex);
        }
        finally
        {
            SetButtonsIsEnabled(true, false, false);
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
            SelectedHashs = HashOptions!.Where(i => i.IsChecked).Select(i => i.Hash).ToList(),
            State = HashTaskState.Waiting,
        };
        _hashTaskId++;
        return hashTask;
    }

    private void SetButtonsIsEnabled(bool startBtn, bool resetBtn, bool cancelBtn)
    {
        StartButton.IsEnabled = startBtn;
        ResetButton.IsEnabled = resetBtn;
        CancelButton.IsEnabled = cancelBtn;
    }
}
