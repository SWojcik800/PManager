using ProcessManager.App.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DictionaryEditDialog.xaml
    /// </summary>
    public partial class DictionaryEditDialog : Window, IDialog
    {
        private readonly DictionaryEditDialogViewModel _viewModel;
        public DictionaryEditDialog(DictionaryEditDialogViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Dialog = this;
            InitializeComponent();
            DataContext = _viewModel;
        }

        public void Accept()
        {
            this.DialogResult = true;
        }

        public void Cancel()
        {
            this.DialogResult = false;
        }

        public void Dispose()
        {
            Close();
        }

        public bool Show()
        {
            return ShowDialog() == true;
        }




    }
}
