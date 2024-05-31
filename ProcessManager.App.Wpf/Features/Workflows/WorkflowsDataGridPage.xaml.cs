using System.Windows.Controls;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;
using ProcessManager.App.Wpf.Features.Workflows.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class WorkflowsDataGridPage : Page
{
    private readonly INavigationService _navigationService;
    private readonly IWorkflowService _workflowService;

    public WorkflowsDataGridPage(WorkflowsDataGridViewModel viewModel, INavigationService navigationService, IWorkflowService workflowService)
    {
        InitializeComponent();
        DataContext = viewModel;
        _navigationService = navigationService;
        _workflowService = workflowService;
    }

    private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _navigationService.NavigateTo(typeof(WorkflowEditViewViewModel).FullName);
    }

    private async void MenuItem_Click_1(object sender, System.Windows.RoutedEventArgs e)
    {
        var selectedWorkflow = workflowsDataGrid.SelectedItem;

        if(selectedWorkflow is not null)
        {
            var selectedWorkflowObj = (Workflow)selectedWorkflow;
            var workflow = await _workflowService.GetById(selectedWorkflowObj.Id);
            _navigationService.NavigateTo(typeof(WorkflowEditViewViewModel).FullName, workflow);
        }
    }
}
