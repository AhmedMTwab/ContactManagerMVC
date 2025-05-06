using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ContactManager.CutomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomErrorHandlingMiddleware> _logger;


        public CustomErrorHandlingMiddleware(RequestDelegate next, ILogger<CustomErrorHandlingMiddleware> logger)
        {
            _next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", ex.InnerException.GetType().ToString(), ex.Message);
                }
                else
                {
                    _logger.LogError("Error Occurd: {ErrorType} ErrorMessage:{ErrorMessage}", ex.GetType().ToString(), ex.Message);
                }
                httpContext.Response.StatusCode = 500;
                throw;

            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomErrorHandlingMiddleware>();
        }
    }
}

