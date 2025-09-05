using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserManagement.Api.Configuration
{
    public class AddApiKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAllowAnonymousAttribute =
                context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousApiKeyAttribute>().Any() ||
                context.MethodInfo.DeclaringType!.GetCustomAttributes(true).OfType<AllowAnonymousApiKeyAttribute>().Any();

            if (hasAllowAnonymousAttribute)
                return;

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-API-Key",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "API Key required to access this endpoint"
            });
        }
    }
}