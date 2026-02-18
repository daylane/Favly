using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Favly.api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro não tratado.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Erro interno no servidor do Favly. Tente novamente mais tarde.",
                Detailed = exception.Message // Em produção, você esconderia isso!
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
