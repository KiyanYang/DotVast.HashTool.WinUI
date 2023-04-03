using System.Collections.ObjectModel;
using System.Diagnostics;

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Messages;
using DotVast.HashTool.WinUI.Models.Navigation;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableRecipient, IViewModel, INavigationAware
{
    private readonly ILogger<HomeViewModel> _logger;
    private readonly IPreferencesSettingsService _preferencesSettingsService;
    private readonly IComputeHashService _computeHashService;
    private readonly IDialogService _dialogService;
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;

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
        _preferencesSettingsService.HashSettings.CollectionChanged += (sender, e) =>
        {
            if (sender is ObservableCollection<HashSetting>
                && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(HashSettings));
            }
        };

        _hashTaskService.HashTasks.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(LastHashTask));

        _startingWhenCreateHashTask = _preferencesSettingsService.StartingWhenCreateHashTask;

        IsActive = true;
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

        Messenger.Register<HomeViewModel, HashSettingIsCheckedChangedMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > HashSettingIsCheckedChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsChecked: {m.IsChecked}");
            await _preferencesSettingsService.SaveHashSettingsAsync();
            CreateTaskCommand.NotifyCanExecuteChanged();
        });

        Messenger.Register<HomeViewModel, HashSettingIsEnabledForAppChangedMessage>(this, (r, m) =>
        {
            Debug.WriteLine($"[{DateTime.Now}] HomeViewModel.Messenger > HashSettingIsEnabledForAppChangedMessage");
            Debug.WriteLine($"Hash.Name: {m.HashSetting.Kind}");
            Debug.WriteLine($"IsEnabled: {m.IsEnabledForApp}");
            OnPropertyChanged(nameof(HashSettings));
        });
    }

    #endregion Messenger

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        if (parameter is not HomeParameter homeParameter)
        {
            return;
        }

        if (homeParameter.Kind == HomeParameterKind.EditHashTask && homeParameter.Data is HashTask hashTask)
        {
            InputtingContent = hashTask.Content;
            InputtingMode = hashTask.Mode;
            if (hashTask.Mode == HashTaskMode.Text && hashTask.Encoding is System.Text.Encoding encoding)
            {
                InputtingTextEncoding = TextEncodings.FirstOrDefault(
                    t => t.Name.Equals(encoding.WebName, StringComparison.OrdinalIgnoreCase),
                    TextEncoding.UTF8);
            }
            foreach (var hashSetting in HashSettings)
            {
                hashSetting.IsChecked = hashTask.SelectedHashKinds.Any(h => h == hashSetting.Kind);
            }
        }
    }

    void INavigationAware.OnNavigatedFrom() { }

    #endregion INavigationAware
}
