using System.Windows.Controls;

namespace ProcessManager.App.Wpf.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
