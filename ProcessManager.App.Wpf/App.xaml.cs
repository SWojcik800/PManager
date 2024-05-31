using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Contracts.Views;
using ProcessManager.App.Wpf.Core;
using ProcessManager.App.Wpf.Core.Contracts.Services;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Dialogs;
using ProcessManager.App.Wpf.Features.UserGroups;
using ProcessManager.App.Wpf.Features.Users.ViewModels;
using ProcessManager.App.Wpf.Features.UserWorkflows.Archive;
using ProcessManager.App.Wpf.Features.UserWorkflows.List;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;
using ProcessManager.App.Wpf.Features.Workflows.ViewModels;
using ProcessManager.App.Wpf.Helpers;
using ProcessManager.App.Wpf.Models;
using ProcessManager.App.Wpf.Services;
using ProcessManager.App.Wpf.ViewModels;
using ProcessManager.App.Wpf.Views;

namespace ProcessManager.App.Wpf;

// For more information about application lifecycle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

// WPF UI elements use language en-US by default.
// If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
// Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
public partial class App : Application
{
    private IHost _host;

    public T GetService<T>()
        where T : class
        => _host.Services.GetService(typeof(T)) as T;

    public App()
    {
    }

    

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
        _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration(c =>
                {
                    c.SetBasePath(appLocation);
                })
                .ConfigureServices(ConfigureServices)
                .Build();

        _host.Start();
        MainWindow.WindowState = WindowState.Maximized;
        MainWindow.IsEnabled = false;
        MainWindow.Hide();

        IoCHelper.Init(_host.Services);
        var appDataService = _host.Services.GetService<IPersistAndRestoreService>();
        var dbCreds = appDataService.GetDbConnectionCreds();
        var dbContext = _host.Services.GetService<ApplicationDbContext>();

        if (!dbCreds.CanConnect())
        {
            var setupDialog = _host.Services.GetService<SetupDialog>();
            var result = setupDialog.ShowDialog();
            dbContext.SetDbConnectionCreds(appDataService.GetDbConnectionCreds());
        } else
        {
            dbContext.SetDbConnectionCreds(dbCreds);
        }

        dbContext.InitDb();

        var signInDialog = _host.Services.GetService<SigninDialog>();
        var signInResult = signInDialog.ShowDialog();

        if(signInResult != true)
            MainWindow.Close();
        else
        {
            MainWindow.Show();
            MainWindow.IsEnabled = true;

        }



    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // TODO: Register your services, viewmodels and pages here

        // App Host
        services.AddHostedService<ApplicationHostService>();

        // Activation Handlers

        // Core Services
        services.AddSingleton<IFileService, FileService>();
        services.AddApplicationCore();

        // Services
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddSingleton<UserDataSource>();

        // Views and ViewModels
        services.AddTransient<IShellWindow, ShellWindow>();
        services.AddTransient<ShellViewModel>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainPage>();

        services.AddTransient<UsersGridViewModel>();
        services.AddTransient<UsersGridPage>();

        services.AddTransient<SettingsViewModel>();
        services.AddTransient<SettingsPage>();

        services.AddTransient<SetupDialogViewModel>();
        services.AddTransient<SetupDialog>();

        services.AddTransient<SignInDialogViewModel>();
        services.AddTransient<SigninDialog>();

        services.AddTransient<UserEditDialogViewModel>();
        services.AddTransient<UserEditDialog>();

        services.AddTransient<DictionariesGridViewModel>();
        services.AddTransient<DictionariesGridPage>();

        services.AddTransient<DictionaryEditDialogViewModel>();
        services.AddTransient<DictionaryEditDialog>();

        services.AddTransient<WorkflowsDataGridViewModel>();
        services.AddTransient<WorkflowsDataGridPage>();

        services.AddTransient<WorkflowEditViewViewModel>();
        services.AddTransient<WorkflowEditViewPage>();

        services.AddTransient<UserGroupsViewModel>();
        services.AddTransient<UserGroupsPage>();

        services.AddTransient<UsersInGroupViewModel>();
        services.AddTransient<UsersInGroupPage>();

        services.AddTransient<UserWorkflowCreateViewModel>();
        services.AddTransient<UserWorkflowCreatePage>();

        services.AddTransient<UserWorkflowCreateDetailViewModel>();
        services.AddTransient<UserWorkflowCreateDetailPage>();

        services.AddTransient<UserWorkflowsDataGridViewModel>();
        services.AddTransient<UserWorkflowsDataGridPage>();

        services.AddTransient<ProcessUserWorkflowViewModel>();
        services.AddTransient<ProcessUserWorkflowPage>();

        services.AddTransient<UserWorkflowsArchiveDataGridViewModel>();
        services.AddTransient<UserWorkflowsArchiveDataGridPage>();

        services.AddTransient<UserWorkflowArchiveDetailsViewModel>();
        services.AddTransient<UserWorkflowArchiveDetailsPage>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}
