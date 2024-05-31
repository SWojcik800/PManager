using ProcessManager.App.Wpf.Core.Settings.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows
{
    public class UserWorkflowStageFieldValueData
    {
        public int Id { get; set; }
        public string FieldCode { get; set; }
        public string FieldValue { get; set; }
        public int UserWorkflowDataId { get; set; }
        public UserWorkflowData UserWorkflowData { get; set; }
    }
}
