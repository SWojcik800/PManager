using ProcessManager.App.Wpf.Contracts.Services;
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
    /// Interaction logic for SetupDialog.xaml
    /// </summary>
    public partial class SetupDialog : Window
    {
        public SetupDialog(SetupDialogViewModel viewModel)
        {
            InitializeComponent();
            viewModel.OnDataSaved = () => {
                DialogResult = true;
            };
            this.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
