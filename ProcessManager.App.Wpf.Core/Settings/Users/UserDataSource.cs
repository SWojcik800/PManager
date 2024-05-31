using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public sealed class UserDataSource : AppDataSource<User>
    {
        public UserDataSource(ApplicationDbContext context) : base(context)
        {
        }
    }
}
