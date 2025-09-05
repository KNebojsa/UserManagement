using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Logger;

namespace UserManagement.Api.Controllers
{
    public class ClientController : ApiBaseController
    {
        private readonly IUserService _userService;

        public ClientController(IUserService userService, Application.Logger.ILogger logger)
            : base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates a user. Gets ApiKey for user.
        /// </summary>
        [HttpPost(Name = "UserAuthentication")]
        public async Task<IActionResult> LogIn([FromBody] UserLoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                LogError("Invalid login request payload.");
                return BadRequest(ModelState);
            }

            try
            {
                LogInfo($"Login attempt for user: {loginRequest.UserName}");
                var result = await _userService.AuthenticateUserAsync(loginRequest.UserName, loginRequest.Password);
                LogInfo($"Login successful for user: {loginRequest.UserName}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse("User login failed.", ex);
            }
        }
    }
}
