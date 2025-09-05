using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Attributes
{
    public class EmailAtrribute : ValidationAttribute
    {
        private static readonly Regex _emailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

        public EmailAtrribute()
        {
            ErrorMessage = "The email address format is invalid.";
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            if (value is string email)
                return _emailRegex.IsMatch(email);

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must be a valid email address (e.g., example@domain.com).";
        }
    }
}
