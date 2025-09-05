using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.DTOs.Responses
{
    public class UserResponse
    {
        /// <summary>
        /// User's Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// User's mobile phone number.
        /// </summary>
        public string? MobileNumber { get; set; }

        /// <summary>
        /// User's language.
        /// </summary>
        public required string Language { get; set; }

        /// <summary>
        /// User's culture.
        /// </summary>
        public required string Culture { get; set; }

        /// <summary>
        /// User's registration date.
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
