using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.WinUI;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient
{
    private readonly ILogger<HomeViewModel> _logger;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly IComputeHashService _computeHashService;
    private readonly IDialogService _dialogService;
    private readonly IHashTaskService _hashTaskService;
    private readonly ManualResetEventSlim _mres = new(true);
    private CancellationTokenSource? _cts;

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IPreferencesSettingsService preferencesSettingsService,
        IComputeHashService computeHashService,
        IDialogService dialogService,
        IHashTaskService hashTaskService)
    {
        _logger = logger;
        _preferencesSettingsService = preferencesSettingsService;
        _computeHashService = computeHashService;
        _dialogService = dialogService;
        _hashTaskService = hashTaskService;

        _computeHashService.AtomProgressChanged += (sender, e) => AtomProgressBar.Val = e;
        _computeHashService.TaskProgressChanged += (sender, e) => (TaskProgressBar.Val, TaskProgressBar.Max) = e;
        _computeHashService.StatusChanged += (sender, e) => App.MainWindow.DispatcherQueue.TryEnqueue(() => SetButtonsStatus(e));

        // 响应哈希选项排序
        _preferencesSettingsService.HashOptions.CollectionChanged += (sender, e) =>
        {
            if (sender is ObservableCollection<HashOption>
                && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(HashOptions));
            }
        };

        IsActive = true;
    }

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<HomeViewModel, FileNotFoundInHashFilesMessage>(this, async (r, m) =>
        {
            await App.MainWindow.DispatcherQueue.EnqueueAsync(() => ShowTipMessage(
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

    private void SetButtonsStatus(ComputeHashStatus status)
    {
        StartTaskCommand.NotifyCanExecuteChanged();
        ResetTaskCommand.NotifyCanExecuteChanged();
        CancelTaskCommand.NotifyCanExecuteChanged();
        switch (status)
        {
            case ComputeHashStatus.Free:
                ResetButton.Content = Localization.Home_Button_Pause;
                AtomProgressBar.ShowPaused = false;
                TaskProgressBar.ShowPaused = false;
                break;
            case ComputeHashStatus.Busy:
                ResetButton.Content = Localization.Home_Button_Pause;
                AtomProgressBar.ShowPaused = false;
                TaskProgressBar.ShowPaused = false;
                break;
            case ComputeHashStatus.Pasue:
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
}
