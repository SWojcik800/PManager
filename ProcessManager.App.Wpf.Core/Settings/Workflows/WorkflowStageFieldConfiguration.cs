using ProcessManager.App.Wpf.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class WorkflowStageFieldConfiguration : IEntity
    {
        private WorkflowStageFieldConfiguration()
        {
            //For EF Core
        }
        public WorkflowStageFieldConfiguration(int workflowStageId, string fieldCode, bool isVisible, bool isEditable)
        {
            WorkflowStageId = workflowStageId;
            FieldCode = fieldCode;
            IsVisible = isVisible;
            IsEditable = isEditable;
        }

        public int Id { get; set; }
        public int WorkflowStageId { get; private set; }
        public WorkflowStage WorkflowStage { get; set; }
        public string FieldCode { get; private set; }
        public bool IsVisible { get; set; }
        public bool IsEditable { get; set; }

    }
}
