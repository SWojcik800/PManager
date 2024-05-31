using ProcessManager.App.Wpf.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows
{
    public sealed class WorkflowFormField : IEntity
    {
        private WorkflowFormField()
        {
            //For EF Core
        }
        public WorkflowFormField(string code, string displayName, FormFieldType type, string displayData)
        {
            Code = code;
            DisplayName = displayName;
            Type = type;
            DisplayData = displayData;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public FormFieldType Type { get; set; }
        public string DisplayData { get; set; }
    }

    public enum FormFieldType
    {
        Text,
        Number,
        Checkbox,
        SelectList,
        Dictionary
    }
}
