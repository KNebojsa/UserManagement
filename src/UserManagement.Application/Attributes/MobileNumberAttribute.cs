using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Attributes
{
    /// <summary>
    /// valid examples "+381 64 123-4567", "0641234567", "+1 (800) 555-1234"
    /// </summary>
    public class MobileNumberAttribute : ValidationAttribute
    {
        private static readonly Regex _mobileRegex = new Regex(
         @"^(\+?\d{1,4}[\s-]?)?(\(?\d{2,4}\)?[\s-]?)?[\d\s-]{5,15}$",
         RegexOptions.Compiled | RegexOptions.CultureInvariant
     );

        public MobileNumberAttribute()
        {
            ErrorMessage = "The mobile number format is invalid.";
        }

        public override bool IsValid(object? value)
        {
            if (value is string input)
                return _mobileRegex.IsMatch(input);

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must be a valid mobile number (e.g., +381 64 123-4567).";
        }
    }
}
