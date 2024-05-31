using System.Windows.Controls;

using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class DictionariesGridPage : Page
{
    public DictionariesGridPage(DictionariesGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
