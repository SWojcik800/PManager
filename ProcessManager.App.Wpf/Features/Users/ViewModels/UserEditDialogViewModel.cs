using CommunityToolkit.Mvvm.ComponentModel;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Features.Users.ViewModels
{
    public partial class UserEditDialogViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        public User User { get; set; }
        public int UserId { get; set; }
        public ObservableCollection<EntityStatus> AvailableStatuses { get; } = new ObservableCollection<EntityStatus>();
        public ObservableCollection<PermissionType> PermissionTypes { get; } = new ObservableCollection<PermissionType>();
        public List<PermissionType> SelectedPermissionsForUser { get; set; } = new List<PermissionType>();
        public UserEditDialogViewModel()
        {
            LoadAvailableStatuses();
            LoadPermissionTypesStatuses();
        }

        private void LoadAvailableStatuses()
        {
            var statuses = Enum.GetValues(typeof(EntityStatus));

            foreach (EntityStatus item in statuses)
            {
                AvailableStatuses.Add(item);
            }
        }

        private void LoadPermissionTypesStatuses()
        {
            var statuses = Enum.GetValues(typeof(PermissionType));

            foreach (PermissionType item in statuses)
            {
                PermissionTypes.Add(item);
            }
        }

        public UserEditDialogViewModel(IUserService userService)
        {
            _userService = userService;

            if (UserId == 0)
                User = new User();
            else
                User = _userService.GetUserById(UserId).Result;

        }
    }
}
