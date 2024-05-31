using System.Windows.Controls;

namespace ProcessManager.App.Wpf.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
