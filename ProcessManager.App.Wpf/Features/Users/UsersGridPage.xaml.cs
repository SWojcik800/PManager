using System.Windows.Controls;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Dialogs;
using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class UsersGridPage : Page
{
    public UsersGridPage(UsersGridViewModel viewModel)
    {
        viewModel.ShowAddDialog = () => UserEditDialog.Add();
        viewModel.ShowEditDialog = (User data) => UserEditDialog.Edit(data);        

        InitializeComponent();

        DataContext = viewModel;
        this.usersGridView.DataContext = viewModel;
    }
}
