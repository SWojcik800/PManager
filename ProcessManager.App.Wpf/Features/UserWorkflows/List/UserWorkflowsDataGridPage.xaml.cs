using System.Windows.Controls;
using ProcessManager.App.Wpf.Features.UserWorkflows.List;

namespace ProcessManager.App.Wpf.Views;

public partial class UserWorkflowsDataGridPage : Page
{
    public UserWorkflowsDataGridPage(UserWorkflowsDataGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
