using System.Windows.Controls;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Features.UserGroups;

namespace ProcessManager.App.Wpf.Views;

public partial class UsersInGroupPage : Page
{
    private UsersInGroupViewModel _viewModel;
    public UsersInGroupPage(UsersInGroupViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        DataContext = _viewModel;
        _viewModel.OnUsersInGroupRefreshed = (List<User> users) =>
        {
            selectedUsersListBox.SelectedItems.Clear();
            var selectedUsers = new List<User>();
            selectedUsers.AddRange(users);
            foreach (var item in selectedUsers)
            {
                selectedUsersListBox.SelectedItems.Add(item);
            }
        };
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listSelectedItems = ((ListBox)sender).SelectedItems;
        var selectedUsers = listSelectedItems.Cast<User>().ToList();
        _viewModel.UsersInGroup?.Clear();
        if (_viewModel.UsersInGroup is null)
            _viewModel.UsersInGroup = new List<User>();
        foreach (var item in selectedUsers)
        {
            _viewModel.UsersInGroup.Add(item);
        }
    }
}
