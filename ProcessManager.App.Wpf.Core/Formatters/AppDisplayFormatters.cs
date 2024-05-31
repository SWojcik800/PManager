using ProcessManager.App.Wpf.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Formatters
{
    public static class AppDisplayFormatters
    {
        public static string ToAppDisplayFormat(this EntityStatus status)
        {
            var statusTranslations = new Dictionary<EntityStatus, string>()
            {
                { EntityStatus.Active, "Aktywny" },
                { EntityStatus.NotActive, "Niektywny" }
            };

            return statusTranslations[status];
        }

        public static string ToAppDisplayFormat(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                return "";

            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}
