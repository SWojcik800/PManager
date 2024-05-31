using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.ViewModels;
using ProcessManager.App.Wpf.Core.Migrations;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;
using System.Collections.ObjectModel;

namespace ProcessManager.App.Wpf.Features.Workflows.ViewModels;

public partial class WorkflowEditViewViewModel : ObservableObject, INavigationAware
{
    private readonly IWorkflowService _service;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    public Action<List<UserGroup>> OnSelectedUserGroupsLoadedFunc;
    public Core.Settings.Workflows.Workflow Workflow { get; set; } = new Core.Settings.Workflows.Workflow();
    public ObservableCollection<UserGroup> UserGroups { get; private set; } = new();
    public ObservableCollection<WorkflowFormField> WorkflowFormFields { get; set; } = new();
    public List<UserGroup> SelectedUserGroups { get; set; } = new();
    public ObservableCollection<FormFieldType> FormFieldTypes { get; private set; } = new ObservableCollection<FormFieldType>();
    public bool CanCodeMaskBeEdited { 
        get {
            return Workflow.Id == 0;
        }
    
    }

    public ObservableCollection<WorkflowStage> WorkflowStages { get; set; } = new();
    public ObservableCollection<WorkflowStageFieldConfiguration> WorkflowStageFields { get; set; } = new();
    public WorkflowEditViewViewModel(IWorkflowService service, IDialogService dialogService,
         INavigationService navigationService)
    {
        _service = service;
        _dialogService = dialogService;
        _navigationService = navigationService;
        //LoadAvailableUserGroups();
        //LoadWorkflowFormFields();
        //LoadWorkflowStages();
        //LoadFormFieldTypes();
    }

    private async Task LoadAvailableUserGroups()
    {
        UserGroups.Clear();
        var userGroups = await _service.GetAvailableUserGroups();

        foreach (var userGroup in userGroups)
        {
            UserGroups.Add(userGroup);
        }

    }

    private void LoadFormFieldTypes()
    {
        FormFieldTypes.Clear();
        foreach (FormFieldType item in Enum.GetValues(typeof(FormFieldType)))
        {
            FormFieldTypes.Add(item);
        }
    }

    public void LoadWorkflowFormFields()
    {
        WorkflowFormFields.Clear();

        foreach (var field in Workflow?.Form?.Fields)
        {
            WorkflowFormFields.Add(field);
        }
    }

    public void LoadWorkflowStages()
    {
        WorkflowStages.Clear();

        foreach (var stage in Workflow.Stages)
        {
            WorkflowStages.Add(stage);
        }
    }

    public void CalculateWorkflowStageFields()
    {
        var isNewWorkflow = Workflow.Id == 0;
        WorkflowStageFields.Clear();

        foreach (var stage in WorkflowStages)
        {
            foreach (var field in WorkflowFormFields)
            {
                if(!isNewWorkflow)
                {
                    var stageFieldConfigs = Workflow.Stages.SelectMany(x => x.Configurations)
                        .ToList();

                    var existingStageFieldConfig = stageFieldConfigs.FirstOrDefault(x => x.FieldCode == field.Code && x.WorkflowStageId == stage.Id);
                    if(existingStageFieldConfig is not null)
                    {
                        WorkflowStageFields.Add(existingStageFieldConfig);
                        continue;
                    }

                }

                var stageId = stage.Id;
                var stageField = new WorkflowStageFieldConfiguration(stage.Id, field.Code, false, false);
                stageField.WorkflowStage = stage;
                WorkflowStageFields.Add(stageField);
            }
        }
    }

    [RelayCommand]
    private void AddNewField()
    {
        WorkflowFormFields.Add(new WorkflowFormField($"FIELD_{WorkflowFormFields.Count + 1}", "Nowe pole", FormFieldType.Text, string.Empty));;
        CalculateWorkflowStageFields();
    }

    [RelayCommand]
    private void AddNewStage()
    {
        var workflowStage = new WorkflowStage();
        workflowStage.Index = WorkflowStages.Any() ? WorkflowStages.Last().Index + 1 : 1;
        workflowStage.Name = $"Nowy etap {workflowStage.Index}";
        workflowStage.Id = -workflowStage.Index;
        WorkflowStages.Add(workflowStage);
        CalculateWorkflowStageFields();
    }

    [RelayCommand]
    private async Task Save()
    {
        Workflow.Form.Fields = WorkflowFormFields.ToList();
        Workflow.Stages = WorkflowStages.ToList();
        Workflow.CanCreateUserGroups = SelectedUserGroups.ToList();
        var isExistingWorkflow = Workflow.Id > 0;
        
        if (!isExistingWorkflow)
        {
            foreach (var stage in Workflow.Stages)
            {
                var configurations = WorkflowStageFields.Where(x => x.WorkflowStageId == stage.Id)
                    .ToList();

                foreach (var configuration in configurations)
                {
                    configuration.Id = 0;
                }

                stage.Configurations = configurations;

                stage.Id = 0;
            }
            await _service.Add(Workflow);
        } else
        {
            foreach (var stage in Workflow.Stages)
            {
                var stageId = stage.Id;
                if (stage.Id < 0)
                {
                    stage.Id = 0;
                }

                var configurations = WorkflowStageFields.Where(x => x.WorkflowStageId == stageId || x.WorkflowStageId == 0)
                    .ToList();

                foreach (var configuration in configurations)
                {
                    if (configuration.Id >= 0)
                        continue;

                    configuration.Id = 0;
                }

                stage.Configurations = configurations;

            }

            await _service.Update(Workflow);
        }

        //await RefreshWorkflowData();

        _dialogService.ShowMessageBox("Zapisano dane");
        _navigationService.NavigateTo(typeof(WorkflowsDataGridViewModel).FullName);

    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is not null)
        {
            var workflow = parameter as Workflow;
            Workflow = workflow;
            await LoadAvailableUserGroups();
            OnSelectedUserGroupsLoadedFunc(Workflow.CanCreateUserGroups);
            await RefreshWorkflowData();
        } else
        {
            await LoadAvailableUserGroups();
        }
    }

    private async Task RefreshWorkflowData()
    {
        LoadWorkflowFormFields();
        LoadWorkflowStages();
        LoadFormFieldTypes();

        if (Workflow.Id != 0)
        {
            CalculateWorkflowStageFields();
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
