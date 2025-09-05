using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Attributes;

namespace UserManagement.Application.DTOs.Requests
{
    public class UpdateUserRequest
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(30)]
        public required string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(30)]
        public required string LastName { get; set; }

        /// <summary>
        /// Desired username.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30, ErrorMessage = "Username must not exceed 30 characters.")]
        public required string UserName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// User's mobile phone number.
        /// </summary>
        [MobileNumber]
        public string? MobileNumber { get; set; }

        /// <summary>
        /// User's language.
        /// </summary>
        [Required(ErrorMessage = "Language is required.")]
        [MaxLength(3)]
        public required string Language { get; set; }

        /// <summary>
        /// User's culture.
        /// </summary>
        [Required(ErrorMessage = "Culture is required.")]
        [MaxLength(10)]
        public required string Culture { get; set; }
    }
}
