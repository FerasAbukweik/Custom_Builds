using custom_Peripherals.MiddleWare;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace custom_Peripherals.MiddleWare
{
    public class GlobalExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleWare> _logger;

        public GlobalExceptionMiddleWare(RequestDelegate next ,
                                         ILogger<GlobalExceptionMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                HttpStatusCode statusCode = ex switch
                {
                    SecurityTokenException => HttpStatusCode.Unauthorized,
                    _ => HttpStatusCode.InternalServerError
                };

                context.Response.ContentType = "Application/Json";
                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    ErrorMessage = ex.Message,
                    statusCode = statusCode
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}


public static class ExtensionMethod
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleWare>();
    }
}