using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Features.UserGroups;
using ProcessManager.App.Wpf.Features.UserWorkflows.Archive;
using ProcessManager.App.Wpf.Features.UserWorkflows.List;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;
using ProcessManager.App.Wpf.Features.Workflows.ViewModels;
using ProcessManager.App.Wpf.ViewModels;
using ProcessManager.App.Wpf.Views;

namespace ProcessManager.App.Wpf.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Configure<MainViewModel, MainPage>();
        Configure<UsersGridViewModel, UsersGridPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<DictionariesGridViewModel, DictionariesGridPage>();
        Configure<WorkflowsDataGridViewModel, WorkflowsDataGridPage>();
        Configure<WorkflowEditViewViewModel, WorkflowEditViewPage>();
        Configure<UserGroupsViewModel, UserGroupsPage>();
        Configure<UsersInGroupViewModel, UsersInGroupPage>();
        Configure<UserWorkflowCreateViewModel, UserWorkflowCreatePage>();
        Configure<UserWorkflowCreateDetailViewModel, UserWorkflowCreateDetailPage>();
        Configure<UserWorkflowsDataGridViewModel, UserWorkflowsDataGridPage>();
        Configure<ProcessUserWorkflowViewModel, ProcessUserWorkflowPage>();
        Configure<UserWorkflowsArchiveDataGridViewModel, UserWorkflowsArchiveDataGridPage>();
        Configure<UserWorkflowArchiveDetailsViewModel, UserWorkflowArchiveDetailsPage>();
    }

    public Type GetPageType(string key)
    {
        Type pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    public Page GetPage(string key)
    {
        var pageType = GetPageType(key);
        return _serviceProvider.GetService(pageType) as Page;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
