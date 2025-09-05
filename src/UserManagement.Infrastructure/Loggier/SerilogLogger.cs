using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure.Logger
{
    public class SerilogLogger : Application.Logger.ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SerilogLogger(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void LogInformation(string message)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var request = httpContext.Request;
            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();
            var clientName = httpContext.Items["ClientName"]?.ToString() ?? "Unknown";
            var hostName = Environment.MachineName;
            var method = $"{request.Method} {request.Path}";
            var time = DateTime.UtcNow;

            Log.Information(
                "{Message}, Time={Time}, ClientIP={ClientIp}, ClientName={ClientName}, Host={Host}, Method={Method}",
                message,
                time,
                clientIp,
                clientName,
                hostName,
                method);
        }

        public void LogError(string message)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var request = httpContext.Request;
            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();
            var clientName = httpContext.Items["ClientName"]?.ToString() ?? "Unknown";
            var hostName = Environment.MachineName;
            var method = $"{request.Method} {request.Path}";
            var time = DateTime.UtcNow;

            Log.Error(
                "{Message}, Time={Time}, ClientIP={ClientIp}, ClientName={ClientName}, Host={Host}, Method={Method}",
                message,
                time,
                clientIp,
                clientName,
                hostName,
                method);
        }
    }
}
