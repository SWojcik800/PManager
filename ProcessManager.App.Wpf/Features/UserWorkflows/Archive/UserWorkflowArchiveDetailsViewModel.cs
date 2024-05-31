using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;

namespace ProcessManager.App.Wpf.Features.UserWorkflows.Archive;

public class UserWorkflowArchiveDetailsViewModel : ObservableObject, INavigationAware
{
    private readonly IUserWorkflowService _userWorkflowService;
    public Action<UserWorkflowData> OnUserWorkflowLoaded { get; set; }
    public UserWorkflowData UserWorkflow { get; set; }

    public UserWorkflowArchiveDetailsViewModel(IUserWorkflowService userWorkflowService)
    {
        _userWorkflowService = userWorkflowService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        var id = (int)parameter;
        var data = await _userWorkflowService.GetById(id);
        UserWorkflow = data;
        OnUserWorkflowLoaded(UserWorkflow);
    }

    public void OnNavigatedFrom()
    {
    }
}
