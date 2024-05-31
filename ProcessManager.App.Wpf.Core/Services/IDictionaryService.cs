using ProcessManager.App.Wpf.Core.Settings.Dictionaries;

namespace ProcessManager.App.Wpf.Core.Services
{
    public interface IDictionaryService
    {
        List<DataDictionary> GetAllDictionaries();
        Task<List<DictionaryItem>> GetDictionaryItems(int dictionaryId);
        Task SaveDictionary(DataDictionary item);
        Task<int> SaveItemChanges(List<DictionaryItem> items);
    }
}