using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;

using ProcessManager.App.Wpf.Contracts.Services;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Features.UserGroups;
using ProcessManager.App.Wpf.Features.UserWorkflows.Archive;
using ProcessManager.App.Wpf.Features.UserWorkflows.List;
using ProcessManager.App.Wpf.Features.UserWorkflows.Process;
using ProcessManager.App.Wpf.Features.Workflows.ViewModels;
using ProcessManager.App.Wpf.Properties;

namespace ProcessManager.App.Wpf.ViewModels;

public class ShellViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;
    private HamburgerMenuItem _selectedMenuItem;
    private HamburgerMenuItem _selectedOptionsMenuItem;
    private RelayCommand _goBackCommand;
    private ICommand _menuItemInvokedCommand;
    private ICommand _optionsMenuItemInvokedCommand;
    private ICommand _loadedCommand;
    private ICommand _unloadedCommand;

    public HamburgerMenuItem SelectedMenuItem
    {
        get { return _selectedMenuItem; }
        set { SetProperty(ref _selectedMenuItem, value); }
    }

    public HamburgerMenuItem SelectedOptionsMenuItem
    {
        get { return _selectedOptionsMenuItem; }
        set { SetProperty(ref _selectedOptionsMenuItem, value); }
    }

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellMainPage, Glyph = "\uE8A5", TargetPageType = typeof(MainViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UsersGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellWorkflowsDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(WorkflowsDataGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellUserGroupsPage, Glyph = "\uE8A5", TargetPageType = typeof(UserGroupsViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowCreatePage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowCreateViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowsDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowsDataGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowsArchiveDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowsArchiveDataGridViewModel) }
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

    public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new RelayCommand(OnMenuItemInvoked));

    public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

    public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

    public ShellViewModel(INavigationService navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
    }

    public void RefreshSidebar()
    {
        //var currentUser = _userService.CurrentUser;
        //var permissions = currentUser.Permissions.Select(x => x.Permission).ToList();
        //MenuItems.Clear();

        //new HamburgerMenuGlyphItem() { Label = Resources.ShellMainPage, Glyph = "\uE8A5", TargetPageType = typeof(MainViewModel) },


        //if (permissions.Contains(PermissionType.Users))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UsersGridViewModel) });
        //if(permissions.Contains(PermissionType.Dictionaries))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellDictionariesGridPage, Glyph = "\uE8A5", TargetPageType = typeof(DictionariesGridViewModel) });
        //if (permissions.Contains(PermissionType.WorkflowTemplates))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellWorkflowsDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(WorkflowsDataGridViewModel) });
        //if(permissions.Contains(PermissionType.UserGroups))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellUserGroupsPage, Glyph = "\uE8A5", TargetPageType = typeof(UserGroupsViewModel) });
        //if (permissions.Contains(PermissionType.AvailableUserWorkflows))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowCreatePage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowCreateViewModel) });
        //if (permissions.Contains(PermissionType.UserWorkflowsToHandle))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowsDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowsDataGridViewModel) });
        //if (permissions.Contains(PermissionType.ArchiveWorkflows))
        //    MenuItems.Add(new HamburgerMenuGlyphItem() { Label = Resources.ShellUserWorkflowsArchiveDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(UserWorkflowsArchiveDataGridViewModel) });
    }

    private void OnLoaded()
    {
        _navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack()
        => _navigationService.CanGoBack;

    private void OnGoBack()
        => _navigationService.GoBack();

    private void OnMenuItemInvoked()
        => NavigateTo(SelectedMenuItem.TargetPageType);

    private void OnOptionsMenuItemInvoked()
        => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }
}
