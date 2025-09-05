using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Attributes;

namespace UserManagement.UnitTests.Attributes
{
    public class MobileNumberAttributeTests
    {
        private readonly MobileNumberAttribute _mobileNumberAttribute;

        public MobileNumberAttributeTests()
        {
            _mobileNumberAttribute = new MobileNumberAttribute();
        }

        [Theory]
        [InlineData("+381 64 123-4567")]
        [InlineData("0641234567")]
        [InlineData("+1 (800) 555-1234")]
        [InlineData("+44 20 7946 0958")]
        [InlineData("+1-555-123-4567")]
        [InlineData("555-123-4567")]
        [InlineData("(555) 123-4567")]
        [InlineData("+381641234567")]
        [InlineData("064 123 4567")]
        [InlineData("+1 800 555 1234")]
        public void IsValid_WithValidMobileNumbers_ShouldReturnTrue(string mobileNumber)
        {
            // Act
            var result = _mobileNumberAttribute.IsValid(mobileNumber);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithNonStringValue_ShouldReturnFalse()
        {
            // Arrange
            var nonStringValue = 123;

            // Act
            var result = _mobileNumberAttribute.IsValid(nonStringValue);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void FormatErrorMessage_ShouldReturnFormattedMessage()
        {
            // Arrange
            var fieldName = "MobileNumber";

            // Act
            var result = _mobileNumberAttribute.FormatErrorMessage(fieldName);

            // Assert
            result.Should().Contain(fieldName);
            result.Should().Contain("valid mobile number");
            result.Should().Contain("+381 64 123-4567");
        }

        [Fact]
        public void ErrorMessage_ShouldBeSetCorrectly()
        {
            // Assert
            _mobileNumberAttribute.ErrorMessage.Should().Be("The mobile number format is invalid.");
        }

        [Theory]
        [InlineData("+381 64 123-4567")]
        [InlineData("0641234567")]
        [InlineData("+1 (800) 555-1234")]
        public void IsValid_WithValidInternationalFormats_ShouldReturnTrue(string mobileNumber)
        {
            // Act
            var result = _mobileNumberAttribute.IsValid(mobileNumber);

            // Assert
            result.Should().BeTrue();
        }
    }
}
