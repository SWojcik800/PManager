using System.Windows.Controls;

using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class UserWorkflowCreatePage : Page
{
    public UserWorkflowCreatePage(UserWorkflowCreateViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
