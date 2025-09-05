using System.Text.RegularExpressions;

namespace UserManagement.Api.Configuration
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Application.Logger.ILogger logger)
        {
            var request = context.Request;

            string body = "";
            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                body = Regex.Replace(body, "(\"password\"\\s*:\\s*\")[^\"]+\"", "$1***\"", RegexOptions.IgnoreCase);
            }

            logger.LogInformation($"Request received: Body={body}");

            await _next(context);
        }
    }
}
