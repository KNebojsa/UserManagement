using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        private static readonly Regex _passwordRegex = new Regex(
            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

        public PasswordAttribute()
        {
            ErrorMessage = "Password must be at least 8 characters long and contain an uppercase letter, lowercase letter, number, and special character.";
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;

            if (value is string password)
                return _passwordRegex.IsMatch(password);

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must contain at least 8 characters, including uppercase, lowercase, digit, and special character.";
        }
    }
}
