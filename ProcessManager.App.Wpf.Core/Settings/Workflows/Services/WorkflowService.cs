using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.DataSource;
using ProcessManager.App.Wpf.Core.Settings.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows.Services
{
    internal sealed class WorkflowService : IWorkflowService
    {
        private readonly ApplicationDbContext _context;

        public WorkflowService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Workflow workflow)
        {
            _context.Workflows.Add(workflow);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Workflow workflow)
        {
            try
            {
                _context.Workflows.Update(workflow);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public async Task<List<Workflow>> GetAllAsync()
            => await _context.Workflows
            .ToListAsync();

        public async Task<Workflow?> GetById(int id)
        {
            var workflow = await _context.Workflows
            .Include(x => x.CanCreateUserGroupsWorkflows)
            .Include(x => x.Stages)
            .ThenInclude(x => x.Configurations)
            .Include(x => x.Form)
            .ThenInclude(x => x.Fields)
            .FirstOrDefaultAsync(x => x.Id == id);

            var users = await _context.Users.ToListAsync();
            var userGroups = await _context.UserGroups.ToListAsync();

            foreach (var stage in workflow.Stages)
            {
                if(stage.AssigneeType is WorkflowStageAssignee.Group)
                {
                    var group = userGroups.FirstOrDefault(x => x.Id == stage.AssigneeId);
                    stage.AssigneeDisplayName = group?.DisplayName;
                }
                if (stage.AssigneeType is WorkflowStageAssignee.SpecificUser)
                {
                    var user = users.FirstOrDefault(x => x.Id == stage.AssigneeId);
                    stage.AssigneeDisplayName = user?.Login;
                }
            }

            return workflow;
        }

        public async Task<List<Workflow>> GetWorkflowsThatUserCanStart(User user)
        {
            var userGroupsIds = user.UserGroups.Select(x => x.Id).ToList();
            var workflows = await _context.Workflows.Include(x => x.CanCreateUserGroupsWorkflows)
                .Where(x => x.CanCreateUserGroupsWorkflows.Any(y => userGroupsIds.Contains(y.UserGroupId)))
                .ToListAsync(); 

            return workflows;
        }

        public async Task<List<UserGroup>> GetAvailableUserGroups()
            => await _context.UserGroups.ToListAsync();

    }
}
