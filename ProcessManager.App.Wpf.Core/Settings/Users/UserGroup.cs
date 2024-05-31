using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public class UserGroup : IEntity
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public List<User> Users { get; set; } = new();
        public int Count { 
            get {
                return Users.Count();
            }
        }
    }
}
