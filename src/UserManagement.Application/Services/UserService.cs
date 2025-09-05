using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.DTOs.Responses;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services
{
    public class UserService(IUserRepository _userRepository,IClientRepository _clientRepository, IPasswordHasher _passwordHasher, IMapper _mapper) : IUserService
    {
        public async Task<UserResponse> AddUserAsync(CreateUserRequest userRequest)
        {
            if (await _userRepository.CheckIfUserNameExistsAsync(userRequest.UserName))
                throw new DuplicateUserNameException(userRequest.UserName);

            if (await _userRepository.CheckIfEmailExistsAsync(userRequest.Email))
                throw new DuplicateEmailException(userRequest.Email);

            var user = _mapper.Map<User>(userRequest);

            user.PasswordHash = _passwordHasher.HashPassword(userRequest.Password);
            var createdUser = await _userRepository.AddUserAsync(user);

            var client = new Client { UserId = user.Id, ApiKey = Guid.NewGuid().ToString(), User = user };
            await _clientRepository.CreateApiKeyAsync(client);

            return _mapper.Map<UserResponse>(createdUser);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);
                
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUserAsync(Guid id,UpdateUserRequest userRequest)
        {
            if (await _userRepository.CheckIfUserNameExistsAsync(userRequest.UserName))
                throw new DuplicateUserNameException(userRequest.UserName);

            if (await _userRepository.CheckIfEmailExistsAsync(userRequest.Email))
                throw new DuplicateEmailException(userRequest.Email);

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            _mapper.Map(userRequest, user);
            var updatedUser = await _userRepository.UpdateUserAsync(user);

            var response = _mapper.Map<UserResponse>(updatedUser);

            return response;
        }

        public async Task<UserLoginResponse> AuthenticateUserAsync(string userName, string password)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
                throw new AuthenticationException("Invalid username or password.");

            var apiKey = await _clientRepository.GetApiKeyAsync(user.Id);
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("User doesn't have an API key.");

            var loginResponse = _mapper.Map<UserLoginResponse>(user);
            loginResponse.ApiKey = apiKey;

            return loginResponse;
        }
    }
}
