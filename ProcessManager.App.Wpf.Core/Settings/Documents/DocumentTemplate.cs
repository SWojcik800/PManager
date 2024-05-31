using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Documents
{
    public sealed class DocumentTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
