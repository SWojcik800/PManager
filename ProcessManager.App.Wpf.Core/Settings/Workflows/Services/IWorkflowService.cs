

using ProcessManager.App.Wpf.Core.Settings.Users;

namespace ProcessManager.App.Wpf.Core.Settings.Workflows.Services
{
    public interface IWorkflowService
    {
        Task Add(Workflow workflow);
        Task<List<Workflow>> GetAllAsync();
        Task<List<UserGroup>> GetAvailableUserGroups();
        Task<Workflow> GetById(int id);
        Task<List<Workflow>> GetWorkflowsThatUserCanStart(User user);
        Task Update(Workflow workflow);
    }
}