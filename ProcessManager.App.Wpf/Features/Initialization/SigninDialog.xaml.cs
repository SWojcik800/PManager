using ProcessManager.App.Wpf.ViewModels;
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

namespace ProcessManager.App.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for SigninDialog.xaml
    /// </summary>
    public partial class SigninDialog : Window
    {
        private SignInDialogViewModel _viewModel;
        public SigninDialog(SignInDialogViewModel viewModel)
        {
            InitializeComponent();
            viewModel.OnSuccessfulSignIn = () => {
                DialogResult = true;
            };
            viewModel.GetPasswordFunc = () =>
            {
                return pwdBox.Password;
            };

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _viewModel.SignInCommand.Execute(null);
        }
    }
}
