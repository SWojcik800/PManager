using System.Windows.Controls;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Features.UserGroups;

namespace ProcessManager.App.Wpf.Views;

public partial class UserGroupsPage : Page
{
    public UserGroupsPage(UserGroupsViewModel viewModel)
    {
        InitializeComponent();
        viewModel.GetSelectedGroupIdFunc = () =>
        {
            var item = (UserGroup)userGroupsDataGrid.SelectedItem;

            return item is not null ? item.Id : 0;
        };
        DataContext = viewModel;
        
    }
}
