using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.Users;

namespace ProcessManager.App.Wpf.Features.UserGroups;

public partial class UsersInGroupViewModel : ObservableObject, INavigationAware
{
    private readonly IUserGroupsService _service;
    private readonly INavigationService _navigationService;
    private int _groupId = 0;
    [ObservableProperty]
    UserGroup userGroup;
    public List<User> UsersInGroup { get; set; } = new List<User>();
    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
    public Action<List<User>> OnUsersInGroupRefreshed { get; set; }
    public UsersInGroupViewModel(IUserGroupsService service, INavigationService navigationService)
    {
        _service = service;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {

        _groupId = parameter is not null ? (int)parameter : 0;
        await LoadUsersList();
        if (_groupId != 0)
        {
            await RefreshSelectedGroup();
        }
        else
        {
            UserGroup = new UserGroup();
        }
    }

    private async Task RefreshSelectedGroup()
    {
        UserGroup = await _service.GetById(_groupId);

        foreach (var user in UserGroup.Users)
        {
            UsersInGroup.Add(user);
        }

        OnUsersInGroupRefreshed(UsersInGroup);
    }

    private async Task LoadUsersList()
    {
        Users.Clear();

        var allUsers = await _service.GetAllUsers();
        foreach (var user in allUsers)
        {
            Users.Add(user);
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        UserGroup.Users = UsersInGroup.ToList();
        var savedGroupId = await _service.SaveGroupChanges(UserGroup);
        _groupId = savedGroupId;
        _navigationService.GoBack();
    }

    [RelayCommand]
    private async Task Cancel()
    {
        _navigationService.GoBack();
    }

    public void OnNavigatedFrom()
    {
    }

}
