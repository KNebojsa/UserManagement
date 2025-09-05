using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Attributes;

namespace UserManagement.UnitTests.Attributes
{
    public class EmailAttributeTests
    {
        private readonly EmailAtrribute _emailAttribute;

        public EmailAttributeTests()
        {
            _emailAttribute = new EmailAtrribute();
        }

        [Fact]
        public void IsValid_WithNullValue_ShouldReturnTrue()
        {
            // Act
            var result = _emailAttribute.IsValid(null);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithNonStringValue_ShouldReturnFalse()
        {
            // Arrange
            var nonStringValue = 123;

            // Act
            var result = _emailAttribute.IsValid(nonStringValue);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void FormatErrorMessage_ShouldReturnFormattedMessage()
        {
            // Arrange
            var fieldName = "Email";

            // Act
            var result = _emailAttribute.FormatErrorMessage(fieldName);

            // Assert
            result.Should().Contain(fieldName);
            result.Should().Contain("valid email address");
            result.Should().Contain("example@domain.com");
        }

        [Fact]
        public void ErrorMessage_ShouldBeSetCorrectly()
        {
            // Assert
            _emailAttribute.ErrorMessage.Should().Be("The email address format is invalid.");
        }

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co.uk")]
        [InlineData("user+tag@example.org")]
        public void IsValid_WithCommonEmailFormats_ShouldReturnTrue(string email)
        {
            // Act
            var result = _emailAttribute.IsValid(email);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("user@")]
        [InlineData("user@.com")]
        public void IsValid_WithInvalidFormats_ShouldReturnFalse(string email)
        {
            // Act
            var result = _emailAttribute.IsValid(email);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("user123@test-domain.com")]
        [InlineData("user@subdomain.example.com")]
        public void IsValid_WithValidComplexEmails_ShouldReturnTrue(string email)
        {
            // Act
            var result = _emailAttribute.IsValid(email);

            // Assert
            result.Should().BeTrue();
        }
    }
}
