using System.Windows.Controls;
using MahApps.Metro.Controls;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Features.UserWorkflows.Archive;

namespace ProcessManager.App.Wpf.Views;

public partial class UserWorkflowArchiveDetailsPage : Page
{
    private readonly IDictionaryService _dictionaryService;
    private UserWorkflowArchiveDetailsViewModel _viewModel;

    public UserWorkflowArchiveDetailsPage(UserWorkflowArchiveDetailsViewModel viewModel, IDictionaryService dictionaryService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.OnUserWorkflowLoaded = InitFormFields;
        DataContext = _viewModel;
        _dictionaryService = dictionaryService;
    }

    private async void InitFormFields(UserWorkflowData data)
    {
        foreach (var field in data.Workflow.Form.Fields)
        {
            var stageFieldConfiguration = data.CurrentStage.Configurations.First(x => x.FieldCode == field.Code);
            if (!stageFieldConfiguration.IsVisible)
                continue;

            var isReadOnly = true;
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

                    }
                    break;
                case Core.Settings.Workflows.FormFieldType.Number:
                    {
                        stageStackPanel.Children.Add(new Label { Content = field.DisplayName });
                        var control = new NumericUpDown { Name = field.Code, IsReadOnly = isReadOnly, IsEnabled = !isReadOnly };
                        stageStackPanel.Children.Add(control);

                        if (fieldValueExists)
                            control.Value = int.Parse(fieldValue);


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

                            if (fieldValueExists)
                                control.SelectedItem = fieldValue;

                            stageStackPanel.Children.Add(control);

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

                    }
                    break;
                default:
                    break;
            }
        }
    }

    private string? FindFieldValue(string code)
    {
        var field = _viewModel.UserWorkflow.FieldValues.FirstOrDefault(x => x.FieldCode == code);

        return field?.FieldValue;
    }
}
