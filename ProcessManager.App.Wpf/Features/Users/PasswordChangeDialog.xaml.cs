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
    public partial class PasswordChangeDialog : Window
    {
        public PasswordChangeDialog()
        {
            InitializeComponent();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {

            string newPassword = newPasswordBox.Password;
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Hasło nie może być puste.");
                return;
            }

            string confirmPassword = confirmPasswordBox.Password;
            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Hasło nie może być puste.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Hasła się nie zgadzają.");
                return;
            }

            DialogResult = true;

        }

        public string GetPlainTextPassword()
            => confirmPasswordBox.Password;
    }
}
