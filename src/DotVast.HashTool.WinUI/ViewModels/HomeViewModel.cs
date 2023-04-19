// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Navigation;

using Microsoft.Extensions.Logging;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class HomeViewModel : SimpleObservableRecipient, IViewModel, INavigationAware
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

        _hashTaskService.HashTasks.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(LastHashTask));

        _startingWhenCreateHashTask = _preferencesSettingsService.StartingWhenCreateHashTask;

        IsActive = true;
    }

    #region Properties

    /// <summary>
    /// 当前界面输入的哈希任务模式.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
    private HashTaskMode _inputtingMode = HashTaskMode.Files;

    /// <summary>
    /// 当前界面输入的哈希任务内容.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
    private string _inputtingContent = string.Empty;

    /// <summary>
    /// 哈希任务模式.
    /// </summary>
    public HashTaskMode[] HashTaskModes { get; } = Enum.GetValues<HashTaskMode>();

    /// <summary>
    /// Hash 选项.
    /// </summary>
    public IList<HashSetting> HashSettings => _preferencesSettingsService.HashSettings.Where(i => i.IsEnabledForApp).ToArray();

    /// <summary>
    /// 最近一次任务.
    /// </summary>
    public HashTask? LastHashTask => _hashTaskService.HashTasks.LastOrDefault();

    #region StartingWhenCreateHashTask

    [ObservableProperty]
    private bool _startingWhenCreateHashTask;

    partial void OnStartingWhenCreateHashTaskChanged(bool value) =>
        _preferencesSettingsService.StartingWhenCreateHashTask = value;

    #endregion StartingWhenCreateHashTask

    #endregion Properties

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.RegisterV<HomeViewModel, IComputeHashService, string>(this, EMT.IComputeHashService_FileNotFound, static (r, _, v) =>
        {
            r._notificationService.Show(new()
            {
                Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning,
                Title = Localization.Tip_FileSkipped_Title,
                Message = string.Format(Localization.Tip_FileSkipped_FileNotFound, v),
                Duration = TimeSpan.FromMilliseconds(3500),
            });
        });

        Messenger.RegisterV<HomeViewModel, HashSetting, bool>(this, EMT.HashSetting_IsChecked, static (r, o, _) =>
        {
            r._preferencesSettingsService.SaveHashSetting(o);
            r.CreateTaskCommand.NotifyCanExecuteChanged();
        });
    }

    #endregion Messenger

    #region INavigationAware

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        OnPropertyChanged(nameof(HashSettings));

        if (parameter is not HomeParameter homeParameter)
        {
            return;
        }

        if (homeParameter.Kind == HomeParameterKind.EditHashTask && homeParameter.Data is HashTask hashTask)
        {
            InputtingContent = hashTask.Content;
            InputtingMode = hashTask.Mode;
            foreach (var hashSetting in HashSettings)
            {
                hashSetting.IsChecked = hashTask.SelectedHashKinds.Any(h => h == hashSetting.Kind);
            }
        }
    }

    void INavigationAware.OnNavigatedFrom() { }

    #endregion INavigationAware
}
