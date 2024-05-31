using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Dialogs
{
    public class DialogUtils
    {
        public enum DataDialogMode
        {
            Add,
            Edit
        }
    }

    public interface IDialog : IDisposable
    {
        public bool Show();
        public void Accept();
        public void Cancel();
    }
}
