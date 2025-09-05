using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Logger;

namespace UserManagement.Api.Controllers
{
    public class UsersController : ApiBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, Application.Logger.ILogger logger)
            : base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                LogError("User ID is empty.");
                return BadRequest("User ID is required.");
            }

            try
            {
                LogInfo($"Fetching user by ID: {id}");
                var user = await _userService.GetUserByIdAsync(id);
                LogInfo($"User retrieved: {id}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Failed to retrieve user.", ex);
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest userModel)
        {
            if (!ModelState.IsValid)
            {
                LogError("Invalid user creation payload.");
                return BadRequest(ModelState);
            }

            try
            {
                LogInfo($"Creating user with email: {userModel.Email}");
                var createdUser = await _userService.AddUserAsync(userModel);
                LogInfo($"User created: {createdUser.Id}");
                return StatusCode(StatusCodes.Status201Created, createdUser);
            }
            catch (Exception ex)
            {
                return ErrorResponse("User creation failed.", ex);
            }
        }

        /// <summary>
        /// Updates an existing user by ID.
        /// </summary>
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest userModel)
        {
            if (!ModelState.IsValid)
            {
                LogError("Invalid user update payload.");
                return BadRequest(ModelState);
            }

            try
            {
                LogInfo($"Updating user with ID: {id}");
                await _userService.UpdateUserAsync(id, userModel);
                LogInfo($"User updated: {id}");
                return Ok("User has been successfully updated.");
            }
            catch (Exception ex)
            {
                return ErrorResponse("User update failed.", ex);
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                LogInfo($"Deleting user with ID: {id}");
                await _userService.DeleteUserAsync(id);
                LogInfo($"User deleted: {id}");
                return Ok("User has been successfully deleted.");
            }
            catch (Exception ex)
            {
                return ErrorResponse("User deletion failed.", ex);
            }
        }
    }
}
