using ProcessManager.App.Wpf.Core.Contracts;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public interface IUserService
    {
        User CurrentUser { get; }

        Task<User> GetUserById(int userId);
        Task<SaveOperationResult<int>> SaveUser(User user);
        Task<bool> SignIn(string username, string password);
    }
}