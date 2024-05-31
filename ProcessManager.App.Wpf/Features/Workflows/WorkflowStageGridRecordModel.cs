using ProcessManager.App.Wpf.Core.Settings.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Features.Workflows
{
    public sealed class WorkflowStageGridRecordModel
    {

        public int StageIndex { get; set; }
        public string StageName { get; set; }
        public string FieldCode { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEditable { get; set; }

        public WorkflowStageGridRecordModel Clone()
            => new WorkflowStageGridRecordModel()
            {
                StageIndex = StageIndex,
                StageName = StageName,
                FieldCode = FieldCode,
                IsVisible = IsVisible,
                IsEditable = IsEditable,
            };
    }
}
