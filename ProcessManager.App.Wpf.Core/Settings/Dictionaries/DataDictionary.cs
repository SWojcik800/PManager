using ProcessManager.App.Wpf.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Dictionaries
{
    public class DataDictionary : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DictionaryItemValueType ItemsValueTypes { get; set; }
        public List<DictionaryItem> DictionaryItems { get; set; }
    }

    public enum DictionaryItemValueType
    {
        Int,
        String,
        DateTime
    }


}
