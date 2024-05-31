using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Shared.Enums;
using ProcessManager.App.Wpf.Dialogs;
using ProcessManager.App.Wpf.Helpers;

namespace ProcessManager.App.Wpf.ViewModels;

public partial class DictionariesGridViewModel : ObservableObject, INavigationAware
{
    private readonly IDictionaryService _service;

    public ObservableCollection<DictionaryItem> Source { get; } = new ObservableCollection<DictionaryItem>();
    public ObservableCollection<EntityStatus> AvailableStatuses { get; } = new ObservableCollection<EntityStatus>();
    private DataDictionary _selectedDictionary = null;
    public DataDictionary SelectedDictionary { 
        get { return _selectedDictionary; } 
        set {
            _selectedDictionary = value;

            if (_selectedDictionary is not null)
                LoadItemsForDictionary(_selectedDictionary.Id);
        }
    }
    public ObservableCollection<DataDictionary> Dictionaries { get; set; } = new();
    public DictionariesGridViewModel(IDictionaryService service)
    {
        _service = service;
        LoadAvailableStatuses();
        LoadDictionariesData();
    }

    private void LoadAvailableStatuses()
    {
        var statuses = Enum.GetValues(typeof(EntityStatus));

        foreach (EntityStatus item in statuses)
        {
            AvailableStatuses.Add(item);
        }
    }

    public void LoadDictionariesData()
    {
        var dictionaries = _service.GetAllDictionaries();
        Dictionaries.Clear();

        if(dictionaries.Any())
        {
            foreach (var dictionary in dictionaries)
            {
                Dictionaries.Add(dictionary);
            }

            SelectedDictionary = Dictionaries.FirstOrDefault();


            if (SelectedDictionary is not null)
                LoadItemsForDictionary(SelectedDictionary.Id);
        }
    }
    public void OnNavigatedTo(object parameter)
    {
        LoadDictionariesData();

    }

    private async Task LoadItemsForDictionary(int dictionaryId)
    {
        var items = await _service.GetDictionaryItems(dictionaryId);
        Source.Clear();

        foreach (var item in items)
        {
            Source.Add(item);
        }
    }

    [RelayCommand]
    private async Task ChangeSelectedDictionary()
    {
        if(SelectedDictionary is not null)
            await LoadItemsForDictionary(SelectedDictionary.Id);
    }

    [RelayCommand]
    private async Task SaveItemsChanges()
    {
        var items = Source.Select(x => x).ToList();

        await _service.SaveItemChanges(items);
    }

    [RelayCommand]
    private void AddNewItem()
    {
        if (SelectedDictionary is not null)
            Source.Add(new DictionaryItem()
            {
                DictionaryId = SelectedDictionary.Id
            });
    }

    [RelayCommand]
    private async Task AddNewDictionary()
    {

        using (var dictionaryDialog = IoCHelper.GetService<DictionaryEditDialog>())
        {
            var dialogResult = dictionaryDialog.Show();
            if(dialogResult)
                LoadDictionariesData(); 
        }       
        
    }


    public void OnNavigatedFrom()
    {
    }
}
