using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient, IViewModel
{
    private const int MillisecondsDelayCreateTask = 750;

    private readonly ILogger<HomeViewModel> _logger;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly IComputeHashService _computeHashService;
    private readonly IDialogService _dialogService;
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly System.Timers.Timer _timer;

    public HomeViewModel(
        ILogger<HomeViewModel> logger,
        IPreferencesSettingsService preferencesSettingsService,
        IComputeHashService computeHashService,
        IDialogService dialogService,
        IHashTaskService hashTaskService,
        INavigationService navigationService,
        INotificationService notificationService)
    {
        _logger = logger;
        _preferencesSettingsService = preferencesSettingsService;
        _computeHashService = computeHashService;
        _dialogService = dialogService;
        _hashTaskService = hashTaskService;
        _navigationService = navigationService;
        _notificationService = notificationService;

        // 响应哈希选项排序
        _preferencesSettingsService.HashOptions.CollectionChanged += (sender, e) =>
        {
            if (sender is ObservableCollection<HashOption>
                && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(HashOptions));
            }
        };

        _hashTaskService.HashTasks.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(LastHashTask));

        _startingWhenCreateHashTask = _preferencesSettingsService.StartingWhenCreateHashTask;

        IsActive = true;

        _timer = new();
        InitializeCreateTaskTimer();
    }

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<HomeViewModel, FileNotFoundInHashFilesMessage>(this, (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > FileNotFoundInHashFilesMessage");
            Debug.WriteLine($"FilePath: {m.FilePath}");
            _notificationService.Show(new()
            {
                Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning,
                Title = Localization.Tip_FileSkipped_Title,
                Message = string.Format(Localization.Tip_FileSkipped_FileNotFound, m.FilePath),
                Duration = TimeSpan.FromMilliseconds(3500),
            });
        });

        Messenger.Register<HomeViewModel, EditTaskMessage>(this, (r, m) =>
        {
            InputtingContent = m.HashTask.Content;
            InputtingMode = m.HashTask.Mode;
            if (m.HashTask.Mode == HashTaskMode.Text && m.HashTask.Encoding is System.Text.Encoding encoding)
            {
                InputtingTextEncoding = TextEncodings.FirstOrDefault(
                    t => t.Name.Equals(encoding.WebName, StringComparison.OrdinalIgnoreCase),
                    TextEncoding.UTF8);
            }
            foreach (var hashOption in HashOptions)
            {
                hashOption.IsChecked = m.HashTask.SelectedHashs.Any(h => h == hashOption.Hash);
            }
            _navigationService.NavigateTo(Constants.PageKeys.HomePage);
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
        IsDelayCreateTask = true;
        CreateTaskCommand.NotifyCanExecuteChanged();
        await Task.Delay(MillisecondsDelayCreateTask);
        IsDelayCreateTask = false;
        CreateTaskCommand.NotifyCanExecuteChanged();
    }
}
