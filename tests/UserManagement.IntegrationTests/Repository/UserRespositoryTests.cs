using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Repository;
using UserManagement.IntegrationTests.Context;
using Xunit;

namespace UserManagement.IntegrationTests.Repository;

public class UserRepositoryTests
{
    private User CreateSampleUser(Guid? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            UserName = "testuser",
            PasswordHash = "hashed_password",
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            MobileNumber = "1234567890",
            Language = "en",
            Culture = "en-US",
            DateCreated = DateTime.UtcNow,
            Clients = []
        };
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddUserToDatabase()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("AddUserTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();

        var result = await repo.AddUserAsync(user);

        Assert.NotNull(result);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Single(context.Users);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("GetUserByIdTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await repo.GetUserByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.UserName, result!.UserName);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_ShouldReturnCorrectUser()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("GetUserByUserNameTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await repo.GetUserByUserNameAsync(user.UserName);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldModifyUser()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("UpdateUserTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        user.FirstName = "Updated";
        var result = await repo.UpdateUserAsync(user);

        Assert.Equal("Updated", result.FirstName);
        Assert.Equal("Updated", context.Users.First().FirstName);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldRemoveUser()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("DeleteUserTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        await repo.DeleteUserAsync(user);

        Assert.Empty(context.Users);
    }

    [Fact]
    public async Task CheckIfUserNameExistsAsync_ShouldReturnTrueIfExists()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("CheckUserNameExistsTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var exists = await repo.CheckIfUserNameExistsAsync(user.UserName);

        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfUserNameExistsAsync_ShouldReturnFalseIfNotExists()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("CheckUserNameNotExistsTest");
        var repo = new UserRepository(context);

        var exists = await repo.CheckIfUserNameExistsAsync("nonexistent");

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfEmailExistsAsync_ShouldReturnTrueIfExists()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("CheckEmailExistsTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var exists = await repo.CheckIfEmailExistsAsync(user.Email);

        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfEmailExistsAsync_ShouldIgnoreExcludedUserId()
    {
        var context = TestDbContextFactory.CreateInMemoryContext("CheckEmailExcludeUserIdTest");
        var repo = new UserRepository(context);
        var user = CreateSampleUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var exists = await repo.CheckIfEmailExistsAsync(user.Email, user.Id);

        Assert.False(exists); // because it's excluded
    }
}
