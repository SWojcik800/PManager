using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.ViewModels
{
    public partial class SetupDialogViewModel : ObservableObject
    {
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly IDialogService _dialogService;
        public Action OnDataSaved = null;

        public SetupDialogViewModel()
        {
            
        }
        public SetupDialogViewModel(IPersistAndRestoreService persistAndRestoreService, IDialogService dialogService)
        {
            _persistAndRestoreService = persistAndRestoreService;
            _dialogService = dialogService;
            DbConnectionCreds = _persistAndRestoreService.GetDbConnectionCreds();
        }

        public DbConnectionCreds DbConnectionCreds { get; set; }      

        [RelayCommand]
        private void SaveData()
        {

            if(DbConnectionCreds.CanConnect())
            {
                var connectionString = DbConnectionCreds.ToConnectionString();
                _persistAndRestoreService.SetConnectionCreds(DbConnectionCreds);
                _persistAndRestoreService.PersistData();

                if (OnDataSaved is not null)
                    OnDataSaved();

            } else
            {
                _dialogService.ShowMessageBox("Nie można połączyć się z podaną bazą danych");
            }
        }
    }
}
