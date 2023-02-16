using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.WinUI;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient
{
    private const int MillisecondsDelayCreateTask = 1000;

    private readonly ILogger<HomeViewModel> _logger;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly IComputeHashService _computeHashService;
    private readonly IDialogService _dialogService;
    private readonly IHashTaskService _hashTaskService;
    private readonly System.Timers.Timer _timer;

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

        // 响应哈希选项排序
        _preferencesSettingsService.HashOptions.CollectionChanged += (sender, e) =>
        {
            if (sender is ObservableCollection<HashOption>
                && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(HashOptions));
            }
        };

        _startingWhenCreateHashTask = _preferencesSettingsService.StartingWhenCreateHashTask;

        IsActive = true;

        _timer = new();
        InitializeCreateTaskTimer();
    }

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<HomeViewModel, FileNotFoundInHashFilesMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > FileNotFoundInHashFilesMessage");
            Debug.WriteLine($"FilePath: {m.Value}");
            await App.MainWindow.DispatcherQueue.EnqueueAsync(() => ShowTipMessage(
                    Localization.Tip_FileSkipped_Title,
                    string.Format(Localization.Tip_FileSkipped_FileNotFound, m.Value)));
        });

        Messenger.Register<HomeViewModel, PropertyChangedMessage<bool>>(this, (r, m) =>
        {
            switch (m.Sender)
            {
                case HashOption hashOption:
                    switch (m.PropertyName)
                    {
                        case nameof(HashOption.IsChecked):
                            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > PropertyChangedMessage[HashOption.IsChecked]");
                            Debug.WriteLine($"Hash.Name: {hashOption.Hash.Name}");
                            Debug.WriteLine($"IsChecked: {hashOption.IsChecked}");
                            CreateTaskCommand.NotifyCanExecuteChanged();
                            break;
                        case nameof(HashOption.IsEnabled):
                            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > PropertyChangedMessage[HashOption.IsEnabled]");
                            Debug.WriteLine($"Hash.Name: {hashOption.Hash.Name}");
                            Debug.WriteLine($"IsEnabled: {hashOption.IsEnabled}");
                            OnPropertyChanged(nameof(HashOptions));
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        });
    }

    #endregion Messenger

    private void InitializeCreateTaskTimer()
    {
        _timer.AutoReset = false;
        _timer.Enabled = false;
        _timer.Interval = 10;
        _timer.Elapsed += (sender, e) => App.MainWindow.DispatcherQueue.TryEnqueue(async () => await DelayCreateTask());
    }

    private async Task DelayCreateTask()
    {
        _isDelayCreateTask = true;
        CreateTaskCommand.NotifyCanExecuteChanged();
        CreateTaskBtn.Content = LocalizationCommon.Created;
        await Task.Delay(MillisecondsDelayCreateTask);
        CreateTaskBtn.Content = LocalizationCommon.Create;
        _isDelayCreateTask = false;
        CreateTaskCommand.NotifyCanExecuteChanged();
    }

    private void ShowTipMessage(string title, string subTitle)
    {
        TipMessage.Title = title;
        TipMessage.Subtitle = subTitle;
        TipMessage.IsOpen = true;
    }
}
