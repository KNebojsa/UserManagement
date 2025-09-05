using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.DTOs.Responses;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        ///// <summary>
        ///// Retrieves a user by id
        ///// </summary>
        ///// <param name="id">The id to search for</param>
        ///// <returns>Result containing user response if found, or failure details</returns>
        Task<UserResponse?> GetUserByIdAsync(Guid id);

        ///// <summary>
        ///// Creates a new user in the system
        ///// </summary>
        ///// <param name="createUserRequest">The user creation request</param>
        ///// <returns>Result containing the created user response or failure details</returns>
        Task<UserResponse> AddUserAsync(CreateUserRequest userRequest);

        ///// <summary>
        ///// Updates an existing user in the system
        ///// </summary>
        ///// <param name="updateUserRequest">The user update request</param>
        ///// <returns>Result containing the updated user response or failure details</returns>
        Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest userRequest);

        ///// <summary>
        ///// Deletes a user from the system
        ///// </summary>
        ///// <param name="id">The user ID to delete</param>
        ///// <returns>Result containing the deletion response or failure details</returns>
        Task DeleteUserAsync(Guid id);

        ///// <summary>
        ///// Authenticate User
        ///// </summary>
        ///// <param name="id">The userName and password</param>
        ///// <returns>Result containing the updated user response or failure details</returns>
        Task<UserLoginResponse> AuthenticateUserAsync(string userName, string password);
    }
}
