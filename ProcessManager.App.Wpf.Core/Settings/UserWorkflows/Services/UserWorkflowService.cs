using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows.Services;
using ProcessManager.App.Wpf.Core.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services
{
    public sealed class UserWorkflowService : IUserWorkflowService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkflowService _workflowService;
        private readonly IUserService _userService;

        public UserWorkflowService(ApplicationDbContext context, IWorkflowService workflowService, IUserService userService)
        {
            _context = context;
            _workflowService = workflowService;
            _userService = userService;
        }

        public async Task<Result<int>> CreateNewUserWorkflow(int workflowId)
        {
            var workflow = await _workflowService.GetById(workflowId);
            var currentUser = _userService.CurrentUser;
            var activeUserWorkflows = await _context.UserWorkflows
                .Where(x => x.UserId == currentUser.Id)
                .ToListAsync();

            var lastUserWorkflowId = await _context.UserWorkflows.Where(x => x.WorkflowId == workflowId)
                .Select(x => x.Id).OrderByDescending(x => x)
                .FirstOrDefaultAsync();

            var result = UserWorkflowAggregate.CreateForUser(activeUserWorkflows, workflow, currentUser, () => DateTime.Now, lastUserWorkflowId);


            if (!result.IsSuccess)
                return Result<int>.Failure(default, result.ErrorMessage);

            var aggregate = result.Value;

            var recordToInsert = aggregate.GetDataToSave();
            await _context.AddAsync(recordToInsert);
            await _context.SaveChangesAsync();

            return Result<int>.Success(recordToInsert.Id);
        }

        public async Task<List<UserWorkflowData>> GetUserWorkflowsToProcess()
        {
            var currentUser = _userService.CurrentUser;
            var userWorkflows = await _context.UserWorkflows
                .Include(x => x.Workflow)
                .Include(x => x.CurrentStage)
                .Include(x => x.User)
                .Where(x => x.WorkflowStatus == UserWorkflowDataStatus.New)
                .ToListAsync();

            return userWorkflows.Where(x => UserWorkflowFuncs.CanProcessStage(currentUser, x.CurrentStage, x))
                .ToList();
            
        }

        public async Task<List<UserWorkflowData>> GetCompletedUserWorkflows()
        {
            var currentUser = _userService.CurrentUser;
            var userWorkflows = await _context.UserWorkflows
                .Include(x => x.Workflow)
                .Include(x => x.CurrentStage)
                .Include(x => x.User)
                .Where(x => x.WorkflowStatus == UserWorkflowDataStatus.Complete
                && x.CompletedByUserId == currentUser.Id)
                .ToListAsync();

            return userWorkflows;

        }

        public async Task<UserWorkflowData> GetById(int id)
        {
            var userWorkflow = await _context.UserWorkflows
                .Include(x => x.Workflow)
                .ThenInclude(x => x.Form)
                .ThenInclude(x => x.Fields)
                .Include(x => x.Workflow)
                .ThenInclude(x => x.Stages)
                .Include(x => x.CurrentStage)
                .ThenInclude(x => x.Configurations)
                .Include(x => x.User)
                .Include(x => x.FieldValues)
                .FirstAsync(x => x.Id == id);

            return userWorkflow;
        }

        public async Task<Result<UserWorkflowAggregate>> GetUserWorkflowAggregate(int workflowId, int userWorkflowId)
        {
            var workflow = await _workflowService.GetById(workflowId);
            var currentUser = _userService.CurrentUser;
            var activeUserWorkflows = await _context.UserWorkflows
                .Where(x => x.UserId == currentUser.Id)
                .ToListAsync();

            var userWorkflowData = await GetById(userWorkflowId);

            var aggregateResult = UserWorkflowAggregate.ReCreateFromData(activeUserWorkflows, workflow, currentUser, userWorkflowData, () => DateTime.Now);

            if (!aggregateResult.IsSuccess)
                return Result<UserWorkflowAggregate>.Failure(default, aggregateResult.ErrorMessage);

            return Result<UserWorkflowAggregate>.Success(aggregateResult.Value);
        }

        public async Task<Result<int>> SaveUserWorkflowAggregate(UserWorkflowAggregate aggregate)
        {
            var userWorkflowData = aggregate.GetDataToSave();
            _context.UserWorkflows.Update(userWorkflowData);
            await _context.SaveChangesAsync();

            return Result<int>.Success(userWorkflowData.Id);
        }
    }
}
