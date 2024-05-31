using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Formatters;
using ProcessManager.App.Wpf.Core.Shared.Enums;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public class User : IEntity
    {
        public User()
        {
            CreationTime = DateTime.Now;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreationTime { get; set; }
        public EntityStatus Status { get; set; }
        public List<UserPermission> Permissions { get; set; } = new List<UserPermission>();
        public List<UserGroup> UserGroups { get; set; }
        public string StatusDisplayName
        {
            get
            {
                return Status.ToAppDisplayFormat();
            }
        }

        public void SetPasswordHash(string password)
        {
            PasswordHash = PasswordHelper.HashPassword(password);
        }

        public bool HasPermission(PermissionType permission)
            => Permissions.Any(p => p.Permission == permission);

    }
}
