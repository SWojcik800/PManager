using ProcessManager.App.Wpf.Core.Shared.Results;

namespace ProcessManager.App.Wpf.Core.Settings.UserWorkflows.Services
{
    public interface IUserWorkflowService
    {
        Task<Result<int>> CreateNewUserWorkflow(int workflowId);
        Task<UserWorkflowData> GetById(int id);
        Task<List<UserWorkflowData>> GetCompletedUserWorkflows();
        Task<Result<UserWorkflowAggregate>> GetUserWorkflowAggregate(int workflowId, int userWorkflowId);
        Task<List<UserWorkflowData>> GetUserWorkflowsToProcess();
        Task<Result<int>> SaveUserWorkflowAggregate(UserWorkflowAggregate aggregate);
    }
}