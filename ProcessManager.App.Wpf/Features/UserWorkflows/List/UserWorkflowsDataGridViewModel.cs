using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;

namespace ProcessManager.App.Wpf.Features.UserWorkflows.List;

public partial class UserWorkflowsDataGridViewModel : ObservableObject, INavigationAware
{
    private readonly IUserWorkflowService _userWorkflowService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<UserWorkflowData> Source { get; } = new ObservableCollection<UserWorkflowData>();
    public UserWorkflowData SelectedItem { get; set; }
    public UserWorkflowsDataGridViewModel(IUserWorkflowService userWorkflowService, INavigationService navigationService)
    {
        _userWorkflowService = userWorkflowService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Replace this with your actual data
        var data = await _userWorkflowService.GetUserWorkflowsToProcess();

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
        _navigationService.NavigateTo(typeof(ProcessUserWorkflowViewModel).FullName, SelectedItem?.Id);
    }
}
