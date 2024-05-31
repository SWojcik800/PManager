using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.Users;

namespace ProcessManager.App.Wpf.Features.UserGroups;

public partial class UserGroupsViewModel : ObservableObject, INavigationAware
{
    private readonly IUserGroupsService _service;
    private readonly INavigationService _navigationService;
    public Func<int> GetSelectedGroupIdFunc { get; set; }
    public ObservableCollection<UserGroup> Source { get; } = new ObservableCollection<UserGroup>();

    public UserGroupsViewModel(IUserGroupsService service, INavigationService navigationService)
    {
        _service = service;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Replace this with your actual data
        var data = await _service.GetAll();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void GoToUserGroup()
    {
        var groupId = GetSelectedGroupIdFunc();

        if(groupId != 0)
            _navigationService.NavigateTo(typeof(UsersInGroupViewModel).FullName, groupId);
    }

    [RelayCommand]
    private void AddNewUserGroup()
    {
        _navigationService.NavigateTo(typeof(UsersInGroupViewModel).FullName, 0);
    }
}
