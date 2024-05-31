using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Features.UserWorkflows.Archive;

namespace ProcessManager.App.Wpf.ViewModels;

public partial class UserWorkflowsArchiveDataGridViewModel : ObservableObject, INavigationAware
{
    private readonly IUserWorkflowService _userWorkflowService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<UserWorkflowData> Source { get; } = new ObservableCollection<UserWorkflowData>();
    public UserWorkflowData SelectedWorkflow { get; set; }

    public UserWorkflowsArchiveDataGridViewModel(IUserWorkflowService userWorkflowService, INavigationService navigationService)
    {
        _userWorkflowService = userWorkflowService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _userWorkflowService.GetCompletedUserWorkflows();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void NavigateToWorkflow()
    {
        if(SelectedWorkflow is not null)
            _navigationService.NavigateTo(typeof(UserWorkflowArchiveDetailsViewModel).FullName, SelectedWorkflow.Id);
    }
}
