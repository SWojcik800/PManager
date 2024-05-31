using System.Windows.Controls;
using MahApps.Metro.Controls;
using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Features.UserWorkflows.List;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;

namespace ProcessManager.App.Wpf.Views;

public partial class ProcessUserWorkflowPage : Page
{
    private readonly IDictionaryService _dictionaryService;
    private readonly IUserWorkflowService _userWorkflowService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private Dictionary<string, Func<string>> _getFieldsValuesFuncs = new Dictionary<string, Func<string>>();
    private ProcessUserWorkflowViewModel _viewModel;
    public ProcessUserWorkflowPage(ProcessUserWorkflowViewModel viewModel,
        IDictionaryService dictionaryService,
        IUserWorkflowService userWorkflowService,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.OnUserWorkflowLoaded = OnUserWorkflowDataLoaded;
        _dictionaryService = dictionaryService;
        _userWorkflowService = userWorkflowService;
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    private async void OnUserWorkflowDataLoaded(UserWorkflowData data)
    {
        this.stageTextBlock.Text = _viewModel.Title;
        await InitFields(data);

        var canCompleteWorkflow = UserWorkflowFuncs.CanCompleteWorkflow(data.Workflow, data.CurrentStage);
        this.forwardButton.IsEnabled = UserWorkflowFuncs.CanForwardStage(data.Workflow, data.CurrentStage) || canCompleteWorkflow;
        this.goBackButton.IsEnabled = UserWorkflowFuncs.CanRevertStage(data.Workflow, data.CurrentStage);

        if (canCompleteWorkflow)
            this.forwardButton.Content = "Zakończ przepływ";
    }

    private async Task InitFields(UserWorkflowData data)
    {
        foreach (var field in data.Workflow.Form.Fields)
        {
            var stageFieldConfiguration = data.CurrentStage.Configurations.First(x => x.FieldCode == field.Code);
            if (!stageFieldConfiguration.IsVisible)
                continue;

            var isReadOnly = !stageFieldConfiguration.IsEditable;
            var fieldValue = FindFieldValue(field.Code);
            var fieldValueExists = !string.IsNullOrEmpty(fieldValue);
            switch (field.Type)
            {

                case Core.Settings.Workflows.FormFieldType.Text:
                    {
                        var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                        stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                        var control = new TextBox { Text = "", Name = field.Code, IsReadOnly = isReadOnly, IsEnabled = !isReadOnly };

                        if (fieldValueExists)
                            control.Text = fieldValue;

                        stageStackPanel.Children.Add(control);

                        _getFieldsValuesFuncs.Add(field.Code, () => control.Text);
                    }
                    break;
                case Core.Settings.Workflows.FormFieldType.Number:
                    {
                        stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                        var control = new NumericUpDown { Name = field.Code, IsReadOnly = isReadOnly, IsEnabled = !isReadOnly };
                        stageStackPanel.Children.Add(control);

                        if (fieldValueExists)
                            control.Value = int.Parse(fieldValue);

                        _getFieldsValuesFuncs.Add(field.Code, () => control.Value.ToString());

                    }
                    break;
                case Core.Settings.Workflows.FormFieldType.Checkbox:
                    {
                        {
                            stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                            var control = new CheckBox { Name = field.Code, IsEnabled = !isReadOnly };

                            if (fieldValueExists)
                                control.IsChecked = fieldValue.ToLower().Trim() == "true";

                            stageStackPanel.Children.Add(control);
                            _getFieldsValuesFuncs.Add(field.Code, () => control.IsChecked.ToString());

                        }
                    }
                    break;
                case Core.Settings.Workflows.FormFieldType.SelectList:
                    {
                        {
                            var options = field.DisplayData.Split(";").ToList();
                            stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                            var control = new ComboBox { Name = field.Code, IsReadOnly = isReadOnly, IsEnabled = !isReadOnly };
                            foreach (var item in options)
                            {
                                control.Items.Add(item);
                            }

                            if(fieldValueExists)
                                control.SelectedItem = fieldValue;

                            stageStackPanel.Children.Add(control);
                            _getFieldsValuesFuncs.Add(field.Code, () => control.SelectedValue.ToString());

                        }

                    }
                    break;
                case Core.Settings.Workflows.FormFieldType.Dictionary:
                    {
                        var dictionaryItems = await _dictionaryService.GetDictionaryItems(int.Parse(field.DisplayData));
                        var options = dictionaryItems;
                        stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                        var control = new ComboBox { Name = field.Code, IsReadOnly = isReadOnly, IsEnabled = !isReadOnly, DisplayMemberPath = nameof(DictionaryItem.Name), SelectedValuePath = nameof(DictionaryItem.Id) };
                        foreach (var item in options)
                        {
                            control.Items.Add(item);
                        }

                        if (fieldValueExists)
                            control.SelectedValue = int.Parse(fieldValue);

                        stageStackPanel.Children.Add(control);
                        _getFieldsValuesFuncs.Add(field.Code, () =>
                        {
                            var selectedItem = control.SelectedItem;

                            if (selectedItem is null)
                                return "";

                            var selectedDictItem = (DictionaryItem)selectedItem;

                            return selectedDictItem.Id.ToString();
                        });

                    }
                    break;
                default:
                    break;
            }
        }
    }

    private async void forwardButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var fieldValues = _getFieldsValuesFuncs.Select(x => new UserWorkflowStageFieldValueData()
        {
            FieldCode = x.Key,
            FieldValue = x.Value()
        }).ToList();

        var aggregateResult = await _userWorkflowService.GetUserWorkflowAggregate(_viewModel.UserWorkflowData.WorkflowId, _viewModel.UserWorkflowData.Id);

        if(!aggregateResult.IsSuccess)
        {
            _dialogService.ShowMessageBox(aggregateResult.ErrorMessage);
            return;
        } 
        var aggregate = aggregateResult.Value;
        var saveFieldValuesResult = aggregate.SaveFieldValues(fieldValues);

        if(!saveFieldValuesResult.IsSuccess)
        {
            _dialogService.ShowMessageBox(saveFieldValuesResult.ErrorMessage);
            return;
        }

        var data = aggregate.GetDataToSave();
        var canCompleteStage = UserWorkflowFuncs.CanCompleteWorkflow(data.Workflow, data.CurrentStage);

        if(canCompleteStage)
        {
            var completeWorkflowResult = aggregate.CompleteWorkflow();

            if (!completeWorkflowResult.IsSuccess)
            {
                _dialogService.ShowMessageBox(completeWorkflowResult.ErrorMessage);
                return;
            }

            var result = await _userWorkflowService.SaveUserWorkflowAggregate(aggregate);

            if (!result.IsSuccess)
            {
                _dialogService.ShowMessageBox(result.ErrorMessage);
                return;
            }

            if (result.IsSuccess)
            {
                _dialogService.ShowMessageBox("Zakończono przepływ");
                _navigationService.NavigateTo(typeof(UserWorkflowsDataGridViewModel).FullName);
            }
        } else
        {
            var forwardToNextStageResult = aggregate.ForwardToNextStage();

            if (!forwardToNextStageResult.IsSuccess)
            {
                _dialogService.ShowMessageBox(forwardToNextStageResult.ErrorMessage);
                return;
            }

            var result = await _userWorkflowService.SaveUserWorkflowAggregate(aggregate);

            if (!result.IsSuccess)
            {
                _dialogService.ShowMessageBox(result.ErrorMessage);
                return;
            }

            if (result.IsSuccess)
            {
                _dialogService.ShowMessageBox("Przekazano do następnego etapu");
                _navigationService.NavigateTo(typeof(UserWorkflowsDataGridViewModel).FullName);
            }
        }

    }
    private async void goBackButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var fieldValues = _getFieldsValuesFuncs.Select(x => new UserWorkflowStageFieldValueData()
        {
            FieldCode = x.Key,
            FieldValue = x.Value()
        }).ToList();

        var aggregateResult = await _userWorkflowService.GetUserWorkflowAggregate(_viewModel.UserWorkflowData.WorkflowId, _viewModel.UserWorkflowData.Id);

        if (!aggregateResult.IsSuccess)
        {
            _dialogService.ShowMessageBox(aggregateResult.ErrorMessage);
            return;
        }
        var aggregate = aggregateResult.Value;
        var saveFieldValuesResult = aggregate.SaveFieldValues(fieldValues);

        if (!saveFieldValuesResult.IsSuccess)
        {
            _dialogService.ShowMessageBox(saveFieldValuesResult.ErrorMessage);
            return;
        }

        var rejectStageResult = aggregate.RejectToPreviousStage();

        if (!rejectStageResult.IsSuccess)
        {
            _dialogService.ShowMessageBox(rejectStageResult.ErrorMessage);
            return;
        }

        var result = await _userWorkflowService.SaveUserWorkflowAggregate(aggregate);

        if (!result.IsSuccess)
        {
            _dialogService.ShowMessageBox(result.ErrorMessage);
            return;
        }

        if(result.IsSuccess)
        {
            _dialogService.ShowMessageBox("Cofnięto do poprzedniego etapu");
            _navigationService.NavigateTo(typeof(UserWorkflowsDataGridViewModel).FullName);
        }
    }

    private string? FindFieldValue(string code)
    {
        var field = _viewModel.UserWorkflowData.FieldValues.FirstOrDefault(x => x.FieldCode == code);

        return field?.FieldValue;
    }
}
