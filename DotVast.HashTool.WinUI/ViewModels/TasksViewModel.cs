using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        InitializeHashTaskCheckables();
        _hashTaskService.HashTasks.CollectionChanged += HashTasks_CollectionChanged;
    }

    public ObservableCollection<HashTaskCheckable> HashTaskCheckables { get; } = new();

    private void InitializeHashTaskCheckables()
    {
        foreach (var h in _hashTaskService.HashTasks)
        {
            HashTaskCheckables.Add(new(h, false));
        }
    }

    private void HashTasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems is System.Collections.IList newItems)
        {
            foreach (var item in newItems)
            {
                if (item is HashTask hashTask)
                {
                    HashTaskCheckables.Add(new(hashTask, false));
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems is System.Collections.IList oldItems)
        {
            for (int i = oldItems.Count - 1; i >= 0; i--)
            {
                HashTaskCheckables.RemoveAt(e.OldStartingIndex);
            }
        }
    }

    #region INavigationAware

    public void OnNavigatedTo(object? parameter)
    {
        IsActive = true;
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
                            Debug.WriteLine($"[{DateTime.Now}] TasksViewModel.Messenger > PropertyChangedMessage[HashTaskCheckable.IsChecked]");
                            Debug.WriteLine($"HashTask.Content: {hashTaskCheckable.HashTask.Content}");
                            Debug.WriteLine($"IsChecked:        {hashTaskCheckable.IsChecked}");
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
        _navigationService.NavigateTo(Constants.PageKeys.ResultsPage, parameter: hashTask);
    }

    public bool CanExecuteSave => HashTaskCheckables?.Any(h => h.IsChecked) ?? false;

    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task SaveAsync()
    {
        FileSavePicker picker = new()
        {
            SuggestedFileName = LocalizationCommon.Result,
            SuggestedStartLocation = PickerLocationId.Desktop,
        };
        picker.FileTypeChoices.Add("JSON", new[] { ".json" });

        var hwnd = HwndExtensions.GetActiveWindow();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var result = await picker.PickSaveFileAsync();
        if (result != null)
        {
            var hashTasks = HashTaskCheckables.Where(h => h.IsChecked).Select(h => h.HashTask);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            };
            var contents = JsonSerializer.Serialize(hashTasks, options);
            await File.WriteAllTextAsync(result.Path, contents);
        }
    }
}
