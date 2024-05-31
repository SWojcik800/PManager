
namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public interface IUserGroupsService
    {
        Task<List<UserGroup>> GetAll();
        Task<List<User>> GetAllUsers();
        Task<UserGroup> GetById(int id);
        Task<List<User>> GetUsersNotInGroup(int id);
        Task SaveChanges(List<UserGroup> userGroups);
        Task<int> SaveGroupChanges(UserGroup group);
    }
}