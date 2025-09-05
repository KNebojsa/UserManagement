using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Security.Authentication;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Logger;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly Application.Logger.ILogger _logger;

        protected ApiBaseController(Application.Logger.ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult ErrorResponse(string message, Exception ex)
        {
            LogError(message, ex);

            var statusCode = ex switch
            {
                DuplicateUserNameException => HttpStatusCode.Conflict,
                DuplicateEmailException => HttpStatusCode.Conflict,
                UserNotFoundException => HttpStatusCode.NotFound,
                AuthenticationException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                message = $"{message} {ex.Message}"
            };

            return new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };
        }

        protected void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        protected void LogError(string message, Exception? ex = null)
        {
            var error = ex != null ? $"{message} | Exception: {ex.Message}" : message;
            _logger.LogError(error);
        }
    }
}
