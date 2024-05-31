using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;

namespace ProcessManager.App.Wpf.Features.UserWorkflows.Process;

public class ProcessUserWorkflowViewModel : ObservableObject, INavigationAware
{
    private readonly IUserService _userService;
    private readonly IUserWorkflowService _userWorkflowService;
    public Action<UserWorkflowData> OnUserWorkflowLoaded { get; set; }
    public UserWorkflowData UserWorkflowData { get; set; }
    public string Title { 
        get {
            return $"{UserWorkflowData.Code} - {UserWorkflowData.CurrentStage.Name}";
        } 
    }

    public ProcessUserWorkflowViewModel(IUserService userService, IUserWorkflowService _userWorkflowService)
    {
        _userService = userService;
        this._userWorkflowService = _userWorkflowService;
    }

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        var id = (int)parameter;

        var result = await _userWorkflowService.GetById(id);
        UserWorkflowData = result;
        OnUserWorkflowLoaded(UserWorkflowData);
    }
}
