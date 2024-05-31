using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Contracts;
using ProcessManager.App.Wpf.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public sealed class UserService : IUserService
    {
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
        }
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SignIn(string username, string password)
        {
            var user = await _context.Users
                .Include(x => x.Permissions)
                .Include(x => x.UserGroups)
                .FirstOrDefaultAsync(x => x.Login == username);

            if (user is null)
                return false;

            if (user.Status == Shared.Enums.EntityStatus.NotActive)
                return false;

            var loginResult = PasswordHelper.VerifyPassword(password, user.PasswordHash);

            if (loginResult)
                _currentUser = user;

            return loginResult;
        }

        public async Task<SaveOperationResult<int>> SaveUser(User user)
        {
            if (user.Id == 0)
            {
                var duplicateLoginExists = await _context.Users.AnyAsync(x => x.Login == user.Login);

                if (duplicateLoginExists)
                    return SaveOperationResult<int>.Fail("Użytkownik o takim loginie już istnieje");

                _context.Add(user);
            }
            else
            {
                _context.Users.Update(user);
            }

            var id = await _context.SaveChangesAsync();

            return SaveOperationResult<int>.Success(id);
        }

        public async Task<User> GetUserById(int userId)
            => await _context.Users.Include(x => x.Permissions)
            .FirstAsync(x => x.Id == userId);


    }
}
