using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public sealed class UserPermission
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public PermissionType Permission { get; set; }
    }

    public enum PermissionType
    {
        Users,
        Dictionaries,
        WorkflowTemplates,
        UserGroups,
        AvailableUserWorkflows,
        UserWorkflowsToHandle,
        ArchiveWorkflows,
    }
}
