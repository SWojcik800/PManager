//using System.IO;
//using System.Reflection;

//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//using ProcessManager.App.Wpf.Contracts.Services;
//using ProcessManager.App.Wpf.Core.Contracts.Services;
//using ProcessManager.App.Wpf.Core.Services;
//using ProcessManager.App.Wpf.Models;
//using ProcessManager.App.Wpf.Services;
//using ProcessManager.App.Wpf.ViewModels;
//using ProcessManager.App.Wpf.Views;

//using Xunit;

//namespace ProcessManager.App.Wpf.Tests.XUnit;

//public class PagesTests
//{
//    private readonly IHost _host;

//    public PagesTests()
//    {
//        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
//        _host = Host.CreateDefaultBuilder()
//            .ConfigureAppConfiguration(c => c.SetBasePath(appLocation))
//            .ConfigureServices(ConfigureServices)
//            .Build();
//    }

//    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
//    {
//        // Core Services
//        services.AddSingleton<IFileService, FileService>();

//        // Services
//        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
//        services.AddSingleton<ISystemService, SystemService>();
//        services.AddSingleton<ISampleDataService, SampleDataService>();
//        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
//        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
//        services.AddSingleton<IPageService, PageService>();
//        services.AddSingleton<INavigationService, NavigationService>();

//        // ViewModels
//        services.AddTransient<SettingsViewModel>();
//        services.AddTransient<MainViewModel>();
//        services.AddTransient<ListDetailsViewModel>();
//        services.AddTransient<UsersGridViewModel>();
//        services.AddTransient<ContentGridViewModel>();

//        // Configuration
//        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
//    }

//    // TODO: Add tests for functionality you add to SettingsViewModel.
//    [Fact]
//    public void TestSettingsViewModelCreation()
//    {
//        var vm = _host.Services.GetService(typeof(SettingsViewModel));
//        Assert.NotNull(vm);
//    }

//    [Fact]
//    public void TestGetSettingsPageType()
//    {
//        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
//        {
//            var pageType = pageService.GetPageType(typeof(SettingsViewModel).FullName);
//            Assert.Equal(typeof(SettingsPage), pageType);
//        }
//        else
//        {
//            Assert.True(false, $"Can't resolve {nameof(IPageService)}");
//        }
//    }

//    // TODO: Add tests for functionality you add to MainViewModel.
//    [Fact]
//    public void TestMainViewModelCreation()
//    {
//        var vm = _host.Services.GetService(typeof(MainViewModel));
//        Assert.NotNull(vm);
//    }

//    [Fact]
//    public void TestGetMainPageType()
//    {
//        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
//        {
//            var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
//            Assert.Equal(typeof(MainPage), pageType);
//        }
//        else
//        {
//            Assert.True(false, $"Can't resolve {nameof(IPageService)}");
//        }
//    }

//    // TODO: Add tests for functionality you add to ListDetailsViewModel.
//    [Fact]
//    public void TestListDetailsViewModelCreation()
//    {
//        var vm = _host.Services.GetService(typeof(ListDetailsViewModel));
//        Assert.NotNull(vm);
//    }

//    [Fact]
//    public void TestGetListDetailsPageType()
//    {
//        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
//        {
//            var pageType = pageService.GetPageType(typeof(ListDetailsViewModel).FullName);
//            Assert.Equal(typeof(ListDetailsPage), pageType);
//        }
//        else
//        {
//            Assert.True(false, $"Can't resolve {nameof(IPageService)}");
//        }
//    }

//    // TODO: Add tests for functionality you add to DataGridViewModel.
//    [Fact]
//    public void TestDataGridViewModelCreation()
//    {
//        var vm = _host.Services.GetService(typeof(UsersGridViewModel));
//        Assert.NotNull(vm);
//    }

//    [Fact]
//    public void TestGetDataGridPageType()
//    {
//        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
//        {
//            var pageType = pageService.GetPageType(typeof(UsersGridViewModel).FullName);
//            Assert.Equal(typeof(UsersGridPage), pageType);
//        }
//        else
//        {
//            Assert.True(false, $"Can't resolve {nameof(IPageService)}");
//        }
//    }

//    // TODO: Add tests for functionality you add to ContentGridViewModel.
//    [Fact]
//    public void TestContentGridViewModelCreation()
//    {
//        var vm = _host.Services.GetService(typeof(ContentGridViewModel));
//        Assert.NotNull(vm);
//    }

//    [Fact]
//    public void TestGetContentGridPageType()
//    {
//        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
//        {
//            var pageType = pageService.GetPageType(typeof(ContentGridViewModel).FullName);
//            Assert.Equal(typeof(ContentGridPage), pageType);
//        }
//        else
//        {
//            Assert.True(false, $"Can't resolve {nameof(IPageService)}");
//        }
//    }
//}
