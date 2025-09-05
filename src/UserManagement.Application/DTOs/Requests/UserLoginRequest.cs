using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Attributes;

namespace UserManagement.Application.DTOs.Requests
{
    public class UserLoginRequest
    {
        /// <summary>
        /// Desired username.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public required string UserName { get; set; }


        /// <summary>
        /// User's password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [Password]
        public required string Password { get; set; }
    }
}
