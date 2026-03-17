using Favly.Domain.Common.Exceptions;
using System.Net;
using System.Text.Json;


namespace Favly.api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger, IHostEnvironment _env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            (HttpStatusCode statusCode, string message, IEnumerable<string>? errors) = exception switch
            {
                DomainException ex => (
                    HttpStatusCode.BadRequest,
                    ex.Message,
                    (IEnumerable<string>?)null),

                NotFoundException ex => (
                    HttpStatusCode.NotFound,
                    ex.Message,
                    (IEnumerable<string>?)null),

                _ => (
                    HttpStatusCode.InternalServerError,
                    "Erro interno no servidor. Tente novamente mais tarde.",
                    (IEnumerable<string>?)null)
            };
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse(
                StatusCode: context.Response.StatusCode,
                Message: message,
                Errors: errors,
                // Detalhes técnicos só aparecem em desenvolvimento
                Detail: _env.IsDevelopment() && statusCode == HttpStatusCode.InternalServerError
                    ? exception.ToString()
                    : null);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            return context.Response.WriteAsync(json);
        }
    }

    // Resposta padronizada para todos os erros
    public record ErrorResponse(
        int StatusCode,
        string Message,
        IEnumerable<string>? Errors,
        string? Detail);
}

