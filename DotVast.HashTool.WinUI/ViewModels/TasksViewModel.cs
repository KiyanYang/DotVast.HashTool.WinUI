using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;

using DotVast.HashTool.WinUI.Contracts.Services;
using DotVast.HashTool.WinUI.Contracts.ViewModels;
using DotVast.HashTool.WinUI.Models;
using DotVast.HashTool.WinUI.Models.Views;

using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class TasksViewModel : ObservableRecipient, INavigationAware
{
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;

    public TasksViewModel(IHashTaskService hashTaskService, INavigationService navigationService)
    {
        _hashTaskService = hashTaskService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    private IList<HashTaskCheckable>? _hashTaskCheckables;

    #region INavigationAware

    public void OnNavigatedTo(object? parameter)
    {
        IsActive = true;
        SetupHashTaskCheckables();
    }

    public void OnNavigatedFrom()
    {
        IsActive = false;
    }

    #endregion INavigationAware

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.Register<TasksViewModel, PropertyChangedMessage<bool>>(this, (r, m) =>
        {
            switch (m.Sender)
            {
                case HashTaskCheckable hashTaskCheckable:
                    switch (m.PropertyName)
                    {
                        case nameof(HashTaskCheckable.IsChecked):
                            Debug.WriteLine($"---------------- {DateTime.Now} -- TasksViewModel.Messenger.PropertyChangedMessage[HashTaskCheckable.IsChecked]");
                            Debug.WriteLine($"HashTask.Content: {hashTaskCheckable.HashTask.Content}");
                            Debug.WriteLine($"IsChecked:{hashTaskCheckable.IsChecked}");
                            SaveCommand.NotifyCanExecuteChanged();
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

    [RelayCommand]
    private void ShowResult(HashTask hashTask)
    {
        _navigationService.NavigateTo(typeof(ResultsViewModel).FullName!, parameter: hashTask);
    }

    public bool CanExecuteSave => HashTaskCheckables?.Any(h => h.IsChecked) ?? false;

    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task SaveAsync()
    {
        FileSavePicker picker = new()
        {
            SuggestedFileName = "Results",
            SuggestedStartLocation = PickerLocationId.Desktop,
        };
        picker.FileTypeChoices.Add("JSON", new[] { ".json" });

        var hwnd = HwndExtensions.GetActiveWindow();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var result = await picker.PickSaveFileAsync();
        if (result != null)
        {
            var hashTasks = HashTaskCheckables!.Where(h => h.IsChecked).Select(h => h.HashTask);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            };
            var contents = JsonSerializer.Serialize(hashTasks, options);
            await File.WriteAllTextAsync(result.Path, contents);
        }
    }

    private void SetupHashTaskCheckables()
    {
        var hashTaskCheckables = new List<HashTaskCheckable>();
        foreach (var h in _hashTaskService.HashTasks)
        {
            var isChecked = HashTaskCheckables?.FirstOrDefault(c => ReferenceEquals(c.HashTask, h))?.IsChecked ?? false;
            hashTaskCheckables.Add(new(h, isChecked));
        }
        HashTaskCheckables = hashTaskCheckables;
    }
}
