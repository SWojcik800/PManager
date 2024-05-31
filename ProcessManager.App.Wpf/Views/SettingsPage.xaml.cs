using System.Windows.Controls;

using ProcessManager.App.Wpf.ViewModels;

namespace ProcessManager.App.Wpf.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
