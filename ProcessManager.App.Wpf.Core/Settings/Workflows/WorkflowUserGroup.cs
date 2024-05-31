using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Settings.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class WorkflowUserGroup : IEntity
    {
        public int Id { get; set; }
        public Workflow Workflow { get; set; }
        public int WorkflowId { get; set; }
        public UserGroup UserGroup { get; set; }
        public int UserGroupId { get; set; }
    }
}
