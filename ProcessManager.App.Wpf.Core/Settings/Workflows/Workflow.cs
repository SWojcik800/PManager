using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Settings.Documents;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class Workflow : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string CodeMask { get; set; } = "{WorkflowTypeId}/{DD}-{MM}-{YYYY}";
        public List<WorkflowUserGroup> CanCreateUserGroupsWorkflows { get; private set; } = new();
        public List<UserGroup> CanCreateUserGroups
        {
            get
            {
                return CanCreateUserGroupsWorkflows.Select(x => x.UserGroup).ToList();
            }
            set
            {
                CanCreateUserGroupsWorkflows = value.Select(x => new WorkflowUserGroup()
                {
                    UserGroup = x,
                }).ToList();

                CanCreateUserGroupsWorkflows.ForEach(x =>
                {
                    x.Workflow = this;
                });
            }
        }
        public WorkflowForm Form { get; set; } = new();
        public List<WorkflowStage> Stages { get; set; } = new();
        public DocumentTemplate? DocumentTemplate { get; set; }
        public int? DocumentTemplateId { get; set; }
        public EntityStatus Status { get; set; }

    }
}
