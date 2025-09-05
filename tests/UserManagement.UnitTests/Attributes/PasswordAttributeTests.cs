using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Attributes;

namespace UserManagement.UnitTests.Attributes
{
    public class PasswordAttributeTests
    {
        private readonly PasswordAttribute _passwordAttribute;

        public PasswordAttributeTests()
        {
            _passwordAttribute = new PasswordAttribute();
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("MyP@ssw0rd")]
        [InlineData("Str0ng#Pass")]
        [InlineData("Test@123")]
        [InlineData("P@ssw0rd!")]
        public void IsValid_WithValidPasswords_ShouldReturnTrue(string password)
        {
            // Act
            var result = _passwordAttribute.IsValid(password);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("password123!")] // Missing uppercase
        [InlineData("PASSWORD123!")] // Missing lowercase
        [InlineData("Password!")] // Missing digit
        [InlineData("Password123")] // Missing special character
        [InlineData("Pass1!")] // Too short
        [InlineData("")] // Empty string
        [InlineData(null)] // Null value
        public void IsValid_WithInvalidPasswords_ShouldReturnFalse(string? password)
        {
            // Act
            var result = _passwordAttribute.IsValid(password);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithNonStringValue_ShouldReturnFalse()
        {
            // Arrange
            var nonStringValue = 123;

            // Act
            var result = _passwordAttribute.IsValid(nonStringValue);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void FormatErrorMessage_ShouldReturnFormattedMessage()
        {
            // Arrange
            var fieldName = "Password";

            // Act
            var result = _passwordAttribute.FormatErrorMessage(fieldName);

            // Assert
            result.Should().Contain(fieldName);
            result.Should().Contain("8 characters");
            result.Should().Contain("uppercase");
            result.Should().Contain("lowercase");
            result.Should().Contain("digit");
            result.Should().Contain("special character");
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("MyP@ssw0rd")]
        [InlineData("Str0ng#Pass")]
        [InlineData("Test@123")]
        public void IsValid_WithValidPasswordsContainingSpecialCharacters_ShouldReturnTrue(string password)
        {
            // Act
            var result = _passwordAttribute.IsValid(password);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("password123!")]
        [InlineData("PASSWORD123!")]
        [InlineData("Password!")]
        [InlineData("Password123")]
        [InlineData("Pass1!")]
        public void IsValid_WithInvalidPasswords_ShouldReturnFalseAndNotThrowException(string password)
        {
            // Act
            var result = _passwordAttribute.IsValid(password);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ErrorMessage_ShouldBeSetCorrectly()
        {
            // Assert
            _passwordAttribute.ErrorMessage.Should().Contain("8 characters");
            _passwordAttribute.ErrorMessage.Should().Contain("uppercase");
            _passwordAttribute.ErrorMessage.Should().Contain("lowercase");
            _passwordAttribute.ErrorMessage.Should().Contain("number");
            _passwordAttribute.ErrorMessage.Should().Contain("special character");
        }
    }
}
