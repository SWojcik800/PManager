using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Formatters;
using ProcessManager.App.Wpf.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Dictionaries
{
    public class DictionaryItem : IEntity
    {
        public int Id { get; set; }
        public int DictionaryItemId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int DictionaryId { get; set; }
        public DataDictionary Dictionary { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplayName
        {
            get
            {
                return Status.ToAppDisplayFormat();
            }
        }
    }

    
}
