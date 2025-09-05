using AutoMapper;
using FluentAssertions;
using Moq;
using System.Security.Authentication;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.DTOs.Responses;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockClientRepository = new Mock<IClientRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserRepository.Object, _mockClientRepository.Object, _mockPasswordHasher.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddUserAsync_WithValidRequest_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Password = "Password123!",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = createUserRequest.FirstName,
                LastName = createUserRequest.LastName,
                UserName = createUserRequest.UserName,
                PasswordHash = "hashedPassword",
                Email = createUserRequest.Email,
                MobileNumber = createUserRequest.MobileNumber,
                Language = createUserRequest.Language,
                Culture = createUserRequest.Culture,
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var userResponse = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Language = user.Language,
                Culture = user.Culture,
                DateCreated = user.DateCreated
            };

            _mockUserRepository.Setup(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.CheckIfEmailExistsAsync(createUserRequest.Email, null))
                .ReturnsAsync(false);
            _mockPasswordHasher.Setup(x => x.HashPassword(createUserRequest.Password))
                .Returns("hashedPassword");
            _mockMapper.Setup(x => x.Map<User>(createUserRequest))
                .Returns(user);
            _mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(user);
            _mockClientRepository.Setup(x => x.CreateApiKeyAsync(It.IsAny<Client>()))
                .ReturnsAsync(Guid.NewGuid());
            _mockMapper.Setup(x => x.Map<UserResponse>(user))
                .Returns(userResponse);

            // Act
            var result = await _userService.AddUserAsync(createUserRequest);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.FirstName.Should().Be(createUserRequest.FirstName);
            result.LastName.Should().Be(createUserRequest.LastName);
            result.UserName.Should().Be(createUserRequest.UserName);
            result.Email.Should().Be(createUserRequest.Email);

            _mockUserRepository.Verify(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName), Times.Once);
            _mockUserRepository.Verify(x => x.CheckIfEmailExistsAsync(createUserRequest.Email, null), Times.Once);
            _mockPasswordHasher.Verify(x => x.HashPassword(createUserRequest.Password), Times.Once);
            _mockUserRepository.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
            _mockClientRepository.Verify(x => x.CreateApiKeyAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_WithDuplicateUserName_ShouldThrowDuplicateUserNameException()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Password = "Password123!",
                Email = "john.doe@example.com",
                Language = "en",
                Culture = "en-US"
            };

            _mockUserRepository.Setup(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DuplicateUserNameException>(
                () => _userService.AddUserAsync(createUserRequest));

            exception.Message.Should().Contain(createUserRequest.UserName);
            _mockUserRepository.Verify(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName), Times.Once);
            _mockUserRepository.Verify(x => x.CheckIfEmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>()), Times.Never);
        }

        [Fact]
        public async Task AddUserAsync_WithDuplicateEmail_ShouldThrowDuplicateEmailException()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Password = "Password123!",
                Email = "john.doe@example.com",
                Language = "en",
                Culture = "en-US"
            };

            _mockUserRepository.Setup(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.CheckIfEmailExistsAsync(createUserRequest.Email, null))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DuplicateEmailException>(
                () => _userService.AddUserAsync(createUserRequest));

            exception.Message.Should().Contain(createUserRequest.Email);
            _mockUserRepository.Verify(x => x.CheckIfUserNameExistsAsync(createUserRequest.UserName), Times.Once);
            _mockUserRepository.Verify(x => x.CheckIfEmailExistsAsync(createUserRequest.Email, null), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithExistingUser_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var userResponse = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName, 
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Language = user.Language,
                Culture = user.Culture,
                DateCreated = user.DateCreated
            };

            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(user);
            _mockMapper.Setup(x => x.Map<UserResponse>(user))
                .Returns(userResponse);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);

            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithNonExistingUser_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(
                () => _userService.GetUserByIdAsync(userId));

            exception.Message.Should().Contain(userId.ToString());
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_WithExistingUser_ShouldReturnUser()
        {
            // Arrange
            var userName = "johndoe";
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = userName,
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var userResponse = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Language = user.Language,
                Culture = user.Culture,
                DateCreated = user.DateCreated
            };

            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync(user);
            _mockMapper.Setup(x => x.Map<UserResponse>(user))
                .Returns(userResponse);

            // Act
            var result = await _userService.GetUserByUserNameAsync(userName);

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be(userName);

            _mockUserRepository.Verify(x => x.GetUserByUserNameAsync(userName), Times.Once);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_WithNonExistingUser_ShouldReturnNull()
        {
            // Arrange
            var userName = "nonexistent";
            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync((User?)null);
            _mockMapper.Setup(x => x.Map<UserResponse>(null))
                .Returns((UserResponse?)null);

            // Act
            var result = await _userService.GetUserByUserNameAsync(userName);

            // Assert
            result.Should().BeNull();
            _mockUserRepository.Verify(x => x.GetUserByUserNameAsync(userName), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidRequest_ShouldUpdateUserSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                UserName = "janesmith",
                Email = "jane.smith@example.com",
                MobileNumber = "+0987654321",
                Language = "en",
                Culture = "en-US"
            };

            var existingUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var updatedUser = new User
            {
                Id = userId,
                FirstName = updateUserRequest.FirstName,
                LastName = updateUserRequest.LastName,
                UserName = updateUserRequest.UserName,
                PasswordHash = "hashedPassword",
                Email = updateUserRequest.Email,
                MobileNumber = updateUserRequest.MobileNumber,
                Language = updateUserRequest.Language,
                Culture = updateUserRequest.Culture,
                DateCreated = existingUser.DateCreated,
                DateModified = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var userResponse = new UserResponse
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                UserName = updatedUser.UserName,
                Email = updatedUser.Email,
                MobileNumber = updatedUser.MobileNumber,
                Language = updatedUser.Language,
                Culture = updatedUser.Culture,
                DateCreated = updatedUser.DateCreated
            };

            _mockUserRepository.Setup(x => x.CheckIfUserNameExistsAsync(updateUserRequest.UserName))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.CheckIfEmailExistsAsync(updateUserRequest.Email, null))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(existingUser);
            _mockUserRepository.Setup(x => x.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(updatedUser);
            _mockMapper.Setup(x => x.Map<UserResponse>(updatedUser))
                .Returns(userResponse);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserRequest);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be(updateUserRequest.FirstName);
            result.LastName.Should().Be(updateUserRequest.LastName);
            result.UserName.Should().Be(updateUserRequest.UserName);
            result.Email.Should().Be(updateUserRequest.Email);

            _mockUserRepository.Verify(x => x.CheckIfUserNameExistsAsync(updateUserRequest.UserName), Times.Once);
            _mockUserRepository.Verify(x => x.CheckIfEmailExistsAsync(updateUserRequest.Email, null), Times.Once);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithNonExistingUser_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                UserName = "janesmith",
                Email = "jane.smith@example.com",
                Language = "en",
                Culture = "en-US"
            };

            _mockUserRepository.Setup(x => x.CheckIfUserNameExistsAsync(updateUserRequest.UserName))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.CheckIfEmailExistsAsync(updateUserRequest.Email, null))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(
                () => _userService.UpdateUserAsync(userId, updateUserRequest));

            exception.Message.Should().Contain(userId.ToString());
        }

        [Fact]
        public async Task DeleteUserAsync_WithExistingUser_ShouldDeleteUserSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.DeleteUserAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(x => x.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WithNonExistingUser_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(
                () => _userService.DeleteUserAsync(userId));

            exception.Message.Should().Contain(userId.ToString());
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(x => x.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithValidCredentials_ShouldReturnLoginResponse()
        {
            // Arrange
            var userName = "johndoe";
            var password = "Password123!";
            var apiKey = "test-api-key";

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = userName,
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            var loginResponse = new UserLoginResponse
            {
                ApiKey = apiKey
            };

            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync(user);
            _mockPasswordHasher.Setup(x => x.VerifyPassword(password, user.PasswordHash))
                .Returns(true);
            _mockClientRepository.Setup(x => x.GetApiKeyAsync(user.Id))
                .ReturnsAsync(apiKey);
            _mockMapper.Setup(x => x.Map<UserLoginResponse>(user))
                .Returns(loginResponse);

            // Act
            var result = await _userService.AuthenticateUserAsync(userName, password);

            // Assert
            result.Should().NotBeNull();
            result.ApiKey.Should().Be(apiKey);

            _mockUserRepository.Verify(x => x.GetUserByUserNameAsync(userName), Times.Once);
            _mockPasswordHasher.Verify(x => x.VerifyPassword(password, user.PasswordHash), Times.Once);
            _mockClientRepository.Verify(x => x.GetApiKeyAsync(user.Id), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithInvalidUsername_ShouldThrowAuthenticationException()
        {
            // Arrange
            var userName = "nonexistent";
            var password = "Password123!";

            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuthenticationException>(
                () => _userService.AuthenticateUserAsync(userName, password));

            exception.Message.Should().Be("Invalid username or password.");
            _mockUserRepository.Verify(x => x.GetUserByUserNameAsync(userName), Times.Once);
            _mockPasswordHasher.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithInvalidPassword_ShouldThrowAuthenticationException()
        {
            // Arrange
            var userName = "johndoe";
            var password = "WrongPassword123!";

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = userName,
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync(user);
            _mockPasswordHasher.Setup(x => x.VerifyPassword(password, user.PasswordHash))
                .Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuthenticationException>(
                () => _userService.AuthenticateUserAsync(userName, password));

            exception.Message.Should().Be("Invalid username or password.");
            _mockUserRepository.Verify(x => x.GetUserByUserNameAsync(userName), Times.Once);
            _mockPasswordHasher.Verify(x => x.VerifyPassword(password, user.PasswordHash), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithMissingApiKey_ShouldThrowException()
        {
            // Arrange
            var userName = "johndoe";
            var password = "Password123!";

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = userName,
                PasswordHash = "hashedPassword",
                Email = "john.doe@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                DateCreated = DateTime.UtcNow,
                Clients = new List<Client>()
            };

            _mockUserRepository.Setup(x => x.GetUserByUserNameAsync(userName))
                .ReturnsAsync(user);
            _mockPasswordHasher.Setup(x => x.VerifyPassword(password, user.PasswordHash))
                .Returns(true);
            _mockClientRepository.Setup(x => x.GetApiKeyAsync(user.Id))
                .ReturnsAsync((string?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.AuthenticateUserAsync(userName, password));

            exception.Message.Should().Be("User doesn't have an API key.");
        }
    }
}
