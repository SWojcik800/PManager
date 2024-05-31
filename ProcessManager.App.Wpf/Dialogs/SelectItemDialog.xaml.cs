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
    /// Interaction logic for SelectItemDialog.xaml
    /// </summary>
    public partial class SelectItemDialog : Window
    {
        protected SelectItemDialog()
        {
            InitializeComponent();
        }

        public static SelectedItemResult Open(List<SelectListItem> items)
        {
            var dialog = new SelectItemDialog();

            dialog.dialogCommboBox.Items.Clear();
            foreach (var item in items)
            {
                dialog.dialogCommboBox.Items.Add(item);
            }

            var dialogResult = dialog.ShowDialog();

            return new SelectedItemResult()
            {
                DialogResult = dialogResult == true,
                SelectedItem = (SelectListItem)dialog.dialogCommboBox.SelectedItem
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(dialogCommboBox.SelectedItem is not null)
            {
                this.DialogResult = true;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

    public sealed class SelectedItemResult
    {
        public bool DialogResult { get; set; }
        public SelectListItem? SelectedItem { get; set; }
    }

    public sealed class SelectListItem
    {
        public SelectListItem()
        {
            
        }
        public SelectListItem(string displayName, object value)
        {
            DisplayName = displayName;
            Value = value;
        }

        public string DisplayName { get; set; }
        public object Value { get; set; }
    }
}
