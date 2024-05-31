using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace ProcessManager.App.Wpf.Models
{
    public class EditDialogResult<T>
    {
        public EditDialogResult(bool isValid, T item)
        {
            SaveData = isValid;
            Item = item;
        }

        public EditDialogResult(bool isValid)
        {
            SaveData = isValid;
        }

        public bool SaveData { get; set; }
        public T Item { get; set; }

        public static EditDialogResult<T> Success(T item)
            => new EditDialogResult<T>(true, item);

        public static EditDialogResult<T> Fail()
            => new EditDialogResult<T>(false);
    }
}
