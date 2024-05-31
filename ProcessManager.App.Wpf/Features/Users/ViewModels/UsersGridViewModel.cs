using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Dialogs;
using ProcessManager.App.Wpf.Models;

namespace ProcessManager.App.Wpf.ViewModels;

public partial class UsersGridViewModel : ObservableObject, INavigationAware
{
    private readonly UserDataSource _dataSource;
    private readonly IDialogService _dialogService;
    private readonly IUserService _userService;

    public ObservableCollection<User> Source { get; } = new ObservableCollection<User>();
    public bool NextPageExists { get; set; }
    public User? SelectedItem { get; set; }
    public Func<User ,EditDialogResult<User>> ShowEditDialog { get; set; }
    public Func<EditDialogResult<User>> ShowAddDialog { get; set; }

    public UsersGridViewModel()
    {
        
    }
    public UsersGridViewModel(UserDataSource dataSource, IDialogService dialogService, IUserService userService)
    {
        _dataSource = dataSource;
        _dialogService = dialogService;
        _userService = userService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        await _dataSource.FetchData();
        NextPageExists = _dataSource.NextPageExists;

        foreach (var item in _dataSource.Data.Items)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private async Task Add()
    {
        var result = ShowAddDialog();
        await SaveUserData(result);
    }

    [RelayCommand]
    private async Task Edit()
    {
        if(SelectedItem is not null)
        {
            var result = ShowEditDialog(SelectedItem);
            await SaveUserData(result);
        }
    }
    private async Task SaveUserData(EditDialogResult<User> result)
    {
        if (result.SaveData)
        {
            var userSaveResult = await _userService.SaveUser(result.Item);

            if (userSaveResult.IsSuccess)
            {
                await _dataSource.FetchData();
                Source.Clear();
                foreach (var item in _dataSource.Data.Items)
                {
                    Source.Add(item);
                }
            }
            else
                _dialogService.ShowMessageBox(userSaveResult.ErrorMessage);
        }
    }

}
