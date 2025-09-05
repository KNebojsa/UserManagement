using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> CheckIfUserNameExistsAsync(string userName);
        Task<bool> CheckIfEmailExistsAsync(string email, Guid? excludeUserId = null);
    }
}
