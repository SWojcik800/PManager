using System.Windows.Controls;

using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class UserWorkflowsArchiveDataGridPage : Page
{
    public UserWorkflowsArchiveDataGridPage(UserWorkflowsArchiveDataGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
