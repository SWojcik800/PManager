using System.Windows.Controls;

using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class UserWorkflowCreateDetailPage : Page
{
    public UserWorkflowCreateDetailPage(UserWorkflowCreateDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
