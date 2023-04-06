using DotVast.HashTool.WinUI.Contracts.Services.Settings;
using DotVast.HashTool.WinUI.Models;
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

        _hashTaskService.HashTasks.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(LastHashTask));

        _startingWhenCreateHashTask = _preferencesSettingsService.StartingWhenCreateHashTask;

        IsActive = true;
    }

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
