using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repository
{
    public class UserRepository(AppDataContext dbContext) : IUserRepository
    {
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await dbContext.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await dbContext.Users.Where(a => a.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserNameExistsAsync(string userName)
        {
            return await dbContext.Users.AsNoTracking().AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> CheckIfEmailExistsAsync(string email, Guid? excludeUserId = null)
        {
            return !excludeUserId.HasValue
                ? await dbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email)
                : await dbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email && u.Id != excludeUserId.Value);
        }
    }
}
