using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Settings.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.ViewModels
{
    public partial class SignInDialogViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly ShellViewModel _shellViewModel;

        public Action OnSuccessfulSignIn { get; set; }
        public Func<string> GetPasswordFunc { get; set; }
        public string UserName { get; set; }
        public SignInDialogViewModel()
        {
            
        }

        public SignInDialogViewModel(IUserService userService, IDialogService dialogService, IPersistAndRestoreService persistAndRestoreService, ShellViewModel shellViewModel)
        {
            _userService = userService;
            _dialogService = dialogService;
            _persistAndRestoreService = persistAndRestoreService;
            _shellViewModel = shellViewModel;
            UserName = _persistAndRestoreService.GetLastSuccessfullLogin();
        }

        [RelayCommand]
        private async Task SignIn()
        {
            var password = GetPasswordFunc();
            var signInResult = await _userService.SignIn(UserName, password);

            if (signInResult)
            {
                _persistAndRestoreService.SaveSuccessfullLogin(UserName);
                _shellViewModel.RefreshSidebar();
                OnSuccessfulSignIn();
            }
            else
                _dialogService.ShowMessageBox("Niepoprawny login lub hasło");
        }


    }
}
