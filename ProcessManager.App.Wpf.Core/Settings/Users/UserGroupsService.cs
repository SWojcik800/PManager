using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Settings.Users
{
    public sealed class UserGroupsService : IUserGroupsService
    {
        private readonly ApplicationDbContext _context;

        public UserGroupsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserGroup>> GetAll()
            => await _context.UserGroups.Include(x => x.Users)
            .ToListAsync();

        public async Task<UserGroup> GetById(int id)
            => await _context.UserGroups.Include(x => x.Users)
            .FirstAsync(x => x.Id == id);

        public async Task<List<User>> GetUsersNotInGroup(int id)
            => await _context.Users.Include(x => x.UserGroups)
            .Where(x =>! x.UserGroups.Any(g => g.Id == id))
            .ToListAsync();
        public async Task<List<User>> GetAllUsers()
            => await _context.Users.Include(x => x.UserGroups)
            .ToListAsync();
        public async Task<int> SaveGroupChanges(UserGroup group)
        {
            if(group.Id == 0)
                _context.UserGroups.Add(group);
            else
                _context.UserGroups.Update(group);
            await _context.SaveChangesAsync();

            return group.Id;
        }
        public async Task SaveChanges(List<UserGroup> userGroups)
        {
            foreach (var group in userGroups)
            {
                if (group.Id == 0)
                    _context.Add(group);
                else
                    _context.Update(group);

            }

            await _context.SaveChangesAsync();
        }
    }
}
