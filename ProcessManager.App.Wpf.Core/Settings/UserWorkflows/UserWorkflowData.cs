using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows
{
    public sealed class UserWorkflowData
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Code { get; set; }
        public Workflow Workflow { get; set; }
        public int? CurrentStageId { get; set; }
        public WorkflowStage? CurrentStage { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public EntityStatus Status { get; set; }
        public UserWorkflowDataStatus WorkflowStatus { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CompletionDate { get; set; }
        public int? CompletedByUserId { get; set; }
        public User? CompletedByUser { get; set; }
        public List<UserWorkflowStageFieldValueData> FieldValues { get; set; }
    }

    public enum UserWorkflowDataStatus
    {
        New,
        Complete
    }
}
