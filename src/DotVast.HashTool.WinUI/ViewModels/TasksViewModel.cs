using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using CommunityToolkit.Mvvm.Input;

using DotVast.HashTool.WinUI.Enums;
using DotVast.HashTool.WinUI.Models;

using Windows.Storage.Pickers;

namespace DotVast.HashTool.WinUI.ViewModels;

public sealed partial class TasksViewModel : ObservableRecipient, IViewModel, INavigationAware
{
    private readonly IHashTaskService _hashTaskService;
    private readonly INavigationService _navigationService;
    private readonly IEnumerable<IExportResolver> _exportResolvers;

    public TasksViewModel(
        IHashTaskService hashTaskService,
        INavigationService navigationService,
        IEnumerable<IExportResolver> exportResolvers)
    {
        _hashTaskService = hashTaskService;
        _navigationService = navigationService;
        _exportResolvers = exportResolvers;
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

    void INavigationAware.OnNavigatedTo(object? parameter)
    {
        IsActive = true;
    }

    void INavigationAware.OnNavigatedFrom()
    {
        IsActive = false;
    }

    #endregion INavigationAware

    #region Messenger

    protected override void OnActivated()
    {
        Messenger.RegisterV<TasksViewModel, HashTaskCheckable, bool>(this, EMT.HashTaskCheckable_IsChecked, static (r, _, _) =>
        {
            r.SaveCommand.NotifyCanExecuteChanged();
        });
    }

    #endregion Messenger

    [RelayCommand]
    private void ShowResult(HashTask hashTask)
    {
        _navigationService.NavigateTo(PageKey.ResultsPage, parameter: hashTask);
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

        var hwnd = WinUIEx.HwndExtensions.GetActiveWindow();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var result = await picker.PickSaveFileAsync();
        if (result != null)
        {
            var hashTasks = HashTaskCheckables.Where(h => h.IsChecked).Select(h => h.HashTask);
            var resolver = _exportResolvers.First(x => x.CanResolve(ExportKind.Text | ExportKind.HashTask, hashTasks));
            await resolver.ExportAsync(result.Path, ExportKind.Text | ExportKind.HashTask, hashTasks);
        }
    }
}
