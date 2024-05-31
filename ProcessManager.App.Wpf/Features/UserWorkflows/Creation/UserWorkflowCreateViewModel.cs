using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;

namespace ProcessManager.App.Wpf.ViewModels;

public class UserWorkflowCreateViewModel : ObservableObject, INavigationAware
{
    private readonly IWorkflowService _workflow;
    private readonly IUserService _userService;
    private readonly IUserWorkflowService _userWorkflowService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private ICommand _navigateToDetailCommand;

    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<Workflow>(NavigateToDetail));

    public ObservableCollection<Workflow> Source { get; } = new ObservableCollection<Workflow>();

    public UserWorkflowCreateViewModel(
        IWorkflowService workflow,
        IUserService userService,
        IUserWorkflowService userWorkflowService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _workflow = workflow;
        _userService = userService;
        _userWorkflowService = userWorkflowService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var currentUser = _userService.CurrentUser;
        var data = await _workflow.GetWorkflowsThatUserCanStart(currentUser);
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private async void NavigateToDetail(Workflow workflow)
    {
        var result = await _userWorkflowService.CreateNewUserWorkflow(workflow.Id);

        if (!result.IsSuccess)
        {
            _dialogService.ShowMessageBox(result.ErrorMessage);
            return;
        }

        _navigationService.NavigateTo(typeof(ProcessUserWorkflowViewModel).FullName, result.Value);
    }
}
