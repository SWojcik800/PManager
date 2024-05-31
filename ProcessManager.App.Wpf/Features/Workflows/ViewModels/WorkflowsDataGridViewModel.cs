using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Models;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;

namespace ProcessManager.App.Wpf.Features.Workflows.ViewModels;

public class WorkflowsDataGridViewModel : ObservableObject, INavigationAware
{
    private readonly IWorkflowService _service;

    public ObservableCollection<Workflow> Source { get; } = new ObservableCollection<Workflow>();

    public WorkflowsDataGridViewModel(IWorkflowService service)
    {
        _service = service;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Replace this with your actual data
        var data = await _service.GetAllAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
