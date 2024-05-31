using ProcessManager.App.Wpf.Models;

namespace ProcessManager.App.Wpf.Contracts.Services;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
