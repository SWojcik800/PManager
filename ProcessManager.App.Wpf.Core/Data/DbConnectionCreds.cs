using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Data
{
    public sealed class DbConnectionCreds
    {

        public string Server { get; set; }
        public string User { get; set; }
        public string InitialCatalog { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }

        public string ToConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();

            if(!string.IsNullOrEmpty(Password))
                builder.Password = Password;
            if (!string.IsNullOrEmpty(User))
                builder.UserID = User;
            if (!string.IsNullOrEmpty(InitialCatalog))
                builder.InitialCatalog = InitialCatalog;
            builder.IntegratedSecurity = IntegratedSecurity;
            if (!string.IsNullOrEmpty(Server))
                builder.DataSource = Server;
            builder.MultipleActiveResultSets = true;

            return builder.ToString();
        }

        public bool CanConnect()
        {

            var isConnectionStringEmpty = string.IsNullOrEmpty(ToConnectionString());

            if (isConnectionStringEmpty)
                return false;

            try
            {
                using (var connection = new SqlConnection(ToConnectionString()))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
