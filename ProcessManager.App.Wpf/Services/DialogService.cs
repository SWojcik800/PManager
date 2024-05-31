using ProcessManager.App.Wpf.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessManager.App.Wpf.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog(Window dialog, object viewModel)
        {
            return dialog.ShowDialog();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}
