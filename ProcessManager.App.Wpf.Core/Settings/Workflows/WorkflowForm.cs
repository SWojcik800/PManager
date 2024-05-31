using ProcessManager.App.Wpf.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class WorkflowForm : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WorkflowFormField> Fields { get; set; } = new List<WorkflowFormField>();
    }
}
