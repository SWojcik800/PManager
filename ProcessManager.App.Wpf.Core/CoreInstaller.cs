using Microsoft.Extensions.DependencyInjection;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.Services;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Settings.Documents;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core
{
    public static class CoreInstaller
    {
        public static void AddApplicationCore(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationDbContext>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDictionaryService, DictionariesService>();
            services.AddSingleton<IWorkflowService, WorkflowService>();
            services.AddSingleton<IUserGroupsService, UserGroupsService>();
            services.AddSingleton<IUserWorkflowService, UserWorkflowService>();
            services.AddSingleton<IDocumentService, DocumentService>();
        }
    }
}
