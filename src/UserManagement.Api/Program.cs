using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using System.Reflection;
using UserManagement.Api.Configuration;
using UserManagement.Application.Mapping;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Logger;

namespace UserManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                options.IncludeXmlComments(apiXmlPath);

                var applicationProjXmlFile = "UserManagement.Application.xml";
                var applicationProjPath = Path.Combine(AppContext.BaseDirectory, applicationProjXmlFile);
                options.IncludeXmlComments(applicationProjPath);

                options.OperationFilter<AddApiKeyHeaderOperationFilter>();
            });

            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly("UserManagement.Infrastructure")
                ));

            builder.Services.AddApplicationServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var response = new { message = "An unexpected error occurred." };
                    await context.Response.WriteAsJsonAsync(response);
                });
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
