using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Dialogs;
using ProcessManager.App.Wpf.Features.Workflows.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class WorkflowEditViewPage : Page
{
    private WorkflowEditViewViewModel _viewModel;
    private readonly IDictionaryService _dictionaryService;
    private readonly IUserService _userService;
    private readonly IUserGroupsService _userGroupsService;

    public WorkflowEditViewPage(WorkflowEditViewViewModel viewModel, IDictionaryService dictionaryService,
        IUserService userService, IUserGroupsService userGroupsService)
    {
        _viewModel = viewModel;
        _dictionaryService = dictionaryService;
        _userService = userService;
        _userGroupsService = userGroupsService;
        InitializeComponent();
        DataContext = _viewModel;

        _viewModel.OnSelectedUserGroupsLoadedFunc = (List<UserGroup> selectedGroups) =>
        {
            CanStartListBox.SelectedItems.Clear();
            var copy = new List<UserGroup>(selectedGroups);

            foreach (var item in copy)
            {
                CanStartListBox.SelectedItems.Add(item);
            }
        };
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listSelectedItems = ((ListBox)sender).SelectedItems;
        _viewModel.SelectedUserGroups = listSelectedItems.Cast<UserGroup>().ToList();
    }
    private void workflowFormFieldsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        _viewModel.CalculateWorkflowStageFields();
    }

    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        _viewModel.CalculateWorkflowStageFields();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        var value = (FormFieldType)comboBox.SelectedValue;
        var isOpen = comboBox.IsDropDownOpen;

        if(isOpen)
        {
            if(value is FormFieldType.Dictionary)
            {
                var dictionaries = _dictionaryService.GetAllDictionaries();
                var selectListItems = dictionaries.Select(x => new SelectListItem(x.Name, x.Id))
                    .ToList();
                var result = SelectItemDialog.Open(selectListItems);

                if(result.DialogResult)
                {
                    var editedFormField = (WorkflowFormField)comboBox.DataContext;
                    editedFormField.DisplayData = result.SelectedItem.Value.ToString();
                    workflowFormFieldsDataGrid.CommitEdit(DataGridEditingUnit.Row, true); 
                    workflowFormFieldsDataGrid.Items.Refresh();

                }
            }
        }
    }

    private async void StagesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        var value = (WorkflowStageAssignee)comboBox.SelectedValue;
        var isOpen = comboBox.IsDropDownOpen;

        if(isOpen)
        {
            switch (value)
            {

                case WorkflowStageAssignee.Creator:
                    {
                        var selectedItem = (WorkflowStage)workflowStagesGrid.SelectedItem;
                        selectedItem.AssigneeDisplayName = "";

                        workflowStagesGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        workflowStagesGrid.Items.Refresh();
                    }
                    break;
                case WorkflowStageAssignee.Group:
                    {
                        var groups = await _userGroupsService.GetAll();
                        var selectListItems = groups.Select(x => new SelectListItem(x.DisplayName, x.Id)).ToList();
                        var result = SelectItemDialog.Open(selectListItems);

                        if(result.DialogResult)
                        {
                            var selectedItem = (WorkflowStage)workflowStagesGrid.SelectedItem;
                            selectedItem.SetAssignee(WorkflowStageAssignee.Group, (int)result.SelectedItem.Value);

                            var group = groups.FirstOrDefault(x => x.Id == (int)result.SelectedItem.Value);
                            if(group is not null)
                            {
                                selectedItem.AssigneeDisplayName = group.DisplayName;
                            }

                            workflowStagesGrid.CommitEdit(DataGridEditingUnit.Row, true);
                            workflowStagesGrid.Items.Refresh();
                        }
                    }
                    break;
                case WorkflowStageAssignee.SpecificUser:
                    {
                        var users = await _userGroupsService.GetAllUsers();
                        var selectListItems = users.Select(x => new SelectListItem(x.Login, x.Id)).ToList();
                        var result = SelectItemDialog.Open(selectListItems);

                        if(result.DialogResult)
                        {
                            var selectedItem = (WorkflowStage)workflowStagesGrid.SelectedItem;
                            selectedItem.SetAssignee(WorkflowStageAssignee.SpecificUser, (int)result.SelectedItem.Value);

                            var user = users.FirstOrDefault(x => x.Id == (int)result.SelectedItem.Value);
                            if (user is not null)
                            {
                                selectedItem.AssigneeDisplayName = user.Login;
                            }

                            workflowStagesGrid.CommitEdit(DataGridEditingUnit.Row, true);
                            workflowStagesGrid.Items.Refresh();
                        }

                    }
                    break;
                default:
                    break;
            }

        }

        
    }
}
