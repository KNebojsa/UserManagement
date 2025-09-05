using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.DTOs.Responses
{
    public class UserLoginResponse
    {
        /// <summary>
        /// User's API key.
        /// </summary>
        public required string ApiKey { get; set; }
    }
}
