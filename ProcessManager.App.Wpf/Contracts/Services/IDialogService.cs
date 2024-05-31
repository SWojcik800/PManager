using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Contracts.Services
{
    public interface IDialogService
    {
        void ShowMessageBox(string message);
    }
}
