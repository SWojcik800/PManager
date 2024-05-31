using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Dialogs;
using System.Collections.ObjectModel;

namespace ProcessManager.App.Wpf.ViewModels
{
    public partial class DictionaryEditDialogViewModel : ObservableObject
    {
        private readonly ProcessManager.App.Wpf.Core.Services.IDictionaryService _service;
        public IDialog Dialog { get; set; }
        public DataDictionary Data { get; set; } = new DataDictionary();
        public ObservableCollection<DictionaryItemValueType> ValueTypes { get; } = new ObservableCollection<DictionaryItemValueType>();
        public DictionaryEditDialogViewModel(ProcessManager.App.Wpf.Core.Services.IDictionaryService service)
        {
            _service = service;
            LoadValueTypes();
        }

        private async Task LoadValueTypes()
        {
            var values = Enum.GetValues(typeof(DictionaryItemValueType));
            foreach (DictionaryItemValueType value in values)
            {
                ValueTypes.Add(value);
            }
        }

        [RelayCommand]
        public async Task Save()
        {

            await _service.SaveDictionary(Data);
            Dialog?.Accept();
        }

        [RelayCommand]
        private void Cancel()
        {
            Dialog?.Cancel();
        }
    }
}
