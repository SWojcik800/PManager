using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Settings.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class WorkflowStage : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public WorkflowStageAssignee AssigneeType { get; set; }
        public int AssigneeId {  get; set; }
        public string AssigneeDisplayName { get; set; }
        public void SetAssignee(WorkflowStageAssignee assignee, int assigneeId)
        {
            AssigneeType = assignee;
            AssigneeId = assigneeId;
        }
        public List<WorkflowStageFieldConfiguration> Configurations { get; set; } = new();
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
    }

    public enum WorkflowStageAssignee
    {
        Creator,
        Group,
        SpecificUser
    }
}
