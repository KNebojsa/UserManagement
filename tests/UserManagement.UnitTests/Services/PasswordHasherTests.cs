using FluentAssertions;
using UserManagement.Application.Services;

namespace UserManagement.UnitTests.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        [Fact]
        public void HashPassword_WithValidPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            hashedPassword.Should().StartWith("$2");
        }

        [Fact]
        public void HashPassword_WithEmptyPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            hashedPassword.Should().StartWith("$2");
        }

        [Fact]
        public void HashPassword_WithSamePassword_ShouldReturnDifferentHashes()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hash1 = _passwordHasher.HashPassword(password);
            var hash2 = _passwordHasher.HashPassword(password);

            // Assert
            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var correctPassword = "TestPassword123!";
            var incorrectPassword = "WrongPassword123!";
            var hashedPassword = _passwordHasher.HashPassword(correctPassword);

            // Act
            var result = _passwordHasher.VerifyPassword(incorrectPassword, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithEmptyPassword_ShouldReturnFalse()
        {
            // Arrange
            var correctPassword = "TestPassword123!";
            var emptyPassword = "";
            var hashedPassword = _passwordHasher.HashPassword(correctPassword);

            // Act
            var result = _passwordHasher.VerifyPassword(emptyPassword, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("SimplePassword")]
        [InlineData("123456789")]
        [InlineData("!@#$%^&*()")]
        [InlineData("")]
        public void HashAndVerifyPassword_WithVariousPasswords_ShouldWorkCorrectly(string password)
        {
            // Arrange & Act
            var hashedPassword = _passwordHasher.HashPassword(password);
            var verificationResult = _passwordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            verificationResult.Should().BeTrue();
        }

        [Fact]
        public void HashPassword_WithSpecialCharacters_ShouldWorkCorrectly()
        {
            // Arrange
            var password = "P@ssw0rd!@#$%^&*()_+-=[]{}|;:,.<>?";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(password);
            var verificationResult = _passwordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            verificationResult.Should().BeTrue();
        }

        [Fact]
        public void HashPassword_WithUnicodeCharacters_ShouldWorkCorrectly()
        {
            // Arrange
            var password = "P@ssw0rd测试123";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(password);
            var verificationResult = _passwordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            verificationResult.Should().BeTrue();
        }
    }
}
