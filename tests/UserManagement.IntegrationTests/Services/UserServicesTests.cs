using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Threading.Tasks;
using UserManagement.Application.Services;
using UserManagement.Application.Interfaces;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.DTOs.Responses;
using UserManagement.Domain.Entities;
using UserManagement.Application.Exceptions;
using System.Security.Authentication;

namespace UserManagement.IntegrationTests.Services;
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private UserService CreateService()
        {
            return new UserService(
                _userRepositoryMock.Object,
                _clientRepositoryMock.Object,
                _passwordHasherMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task AddUserAsync_ShouldCreateUser_WhenDataIsValid()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Password = "password123",
                Email = "john@example.com",
                Culture = "en-US",
                Language = "en",
                MobileNumber = "123456789"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                PasswordHash = "hashed_pw",
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                Language = request.Language,
                Culture = request.Culture,
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var userResponse = new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Language = user.Language,
                Culture = user.Culture,
                DateCreated = user.DateCreated
            };

            _userRepositoryMock
                .Setup(r => r.CheckIfUserNameExistsAsync(It.Is<string>(u => u == request.UserName)))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.CheckIfEmailExistsAsync(It.Is<string>(e => e == request.Email), null))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<User>(request))
                .Returns(user);

            _passwordHasherMock
                .Setup(p => p.HashPassword(request.Password))
                .Returns("hashed_pw");

            _userRepositoryMock
                .Setup(r => r.AddUserAsync(user))
                .ReturnsAsync(user);

            _clientRepositoryMock
                .Setup(c => c.CreateApiKeyAsync(It.IsAny<Client>()))
                .ReturnsAsync(Guid.NewGuid());

            _mapperMock
                .Setup(m => m.Map<UserResponse>(user))
                .Returns(userResponse);

            // Act
            var result = await CreateService().AddUserAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(request.UserName, result.UserName);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrow_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync((User?)null);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => service.DeleteUserAsync(id));
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenFound()
        {
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "johndoe",
                PasswordHash = "hashed_pw",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                MobileNumber = "123456789",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };
            var response = new UserResponse { Id = id, UserName = "johndoe", FirstName = "John", LastName = "Doe", Email = "john@example.com", Culture = "en", Language = "en" };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

            var service = CreateService();

            var result = await service.GetUserByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenFound()
        {
            var userName = "johndoe";
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "johndoe",
                PasswordHash = "hashed_pw",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                MobileNumber = "123456789",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };
            var response = new UserResponse { Id = user.Id, UserName = userName, FirstName = "John", LastName = "Doe", Email = "john@example.com", Culture = "en", Language = "en" };

            _userRepositoryMock.Setup(r => r.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

            var service = CreateService();

            var result = await service.GetUserByUserNameAsync(userName);

            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnLoginResponse_WhenCredentialsAreValid()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "johndoe",
                PasswordHash = "hashed_pw",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                MobileNumber = "123456789",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var loginResponse = new UserLoginResponse
            {
                ApiKey = "api-key-123"
            };

            _userRepositoryMock.Setup(r => r.GetUserByUserNameAsync(user.UserName)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.VerifyPassword("password123", "hashed_pw")).Returns(true);
            _clientRepositoryMock.Setup(c => c.GetApiKeyAsync(user.Id)).ReturnsAsync("api-key-123");
            _mapperMock.Setup(m => m.Map<UserLoginResponse>(user)).Returns(loginResponse);

            var service = CreateService();

            var result = await service.AuthenticateUserAsync(user.UserName, "password123");

            Assert.NotNull(result);
            Assert.Equal("api-key-123", result.ApiKey);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldThrow_WhenInvalidCredentials()
        {
            _userRepositoryMock.Setup(r => r.GetUserByUserNameAsync("wronguser")).ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<AuthenticationException>(() => service.AuthenticateUserAsync("wronguser", "wrongpassword"));
        }
    }
