using CommunityToolkit.Mvvm.Input;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Features.Users.ViewModels;
using ProcessManager.App.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ProcessManager.App.Wpf.Dialogs.DialogUtils;

namespace ProcessManager.App.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for UserEditDialog.xaml
    /// </summary>
    public partial class UserEditDialog : Window
    {
        private UserEditDialogViewModel _viewModel;
        private DataDialogMode _dialogModel = DataDialogMode.Edit;
        public UserEditDialog(UserEditDialogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            this.PermissionsListBox.SelectedItems.Clear();
                
            _viewModel.User.Permissions.Select(x => x.Permission).ToList()
                .ForEach(x =>
                {
                    this.PermissionsListBox.SelectedItems.Add(x);
                });

        }

        public static EditDialogResult<User> Add()
        {
            var vm = new UserEditDialogViewModel();
            vm.User = new User();
            var dialog = new UserEditDialog(vm);
            dialog._dialogModel = DataDialogMode.Add;

            var result = dialog.ShowDialog() == true;

            if (result)
                return EditDialogResult<User>.Success(dialog._viewModel.User);

            return EditDialogResult<User>.Fail();
        }

        public static EditDialogResult<User> Edit(User user)
        {
            var vm = new UserEditDialogViewModel();
            vm.User = user;
            var dialog = new UserEditDialog(vm);
            dialog._dialogModel = DataDialogMode.Edit;

            var result = dialog.ShowDialog() == true;

            if (result)
                return EditDialogResult<User>.Success(dialog._viewModel.User);

            return EditDialogResult<User>.Fail();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(_dialogModel == DataDialogMode.Add)
            {                
                var pwdDialog = new PasswordChangeDialog();
                var result = pwdDialog.ShowDialog();
                var pwd = pwdDialog.GetPlainTextPassword();
                _viewModel.User.SetPasswordHash(pwd);
            }

            _viewModel.User.Permissions = _viewModel.SelectedPermissionsForUser.Select(x => new UserPermission()
            {
                Permission = x
            }).ToList();

            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var pwdDialog = new PasswordChangeDialog();
            var result = pwdDialog.ShowDialog();
            var pwd = pwdDialog.GetPlainTextPassword();
            _viewModel.User.SetPasswordHash(pwd);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listSelectedItems = ((ListBox)sender).SelectedItems;
            _viewModel.SelectedPermissionsForUser = listSelectedItems.Cast<PermissionType>().ToList();
        }
    
    }
}
