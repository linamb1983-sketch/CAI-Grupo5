using Microsoft.AspNetCore.Diagnostics;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve el formato estándar para errores 400.
public class ValidationExceptionHandler : IExceptionHandler
{
    private ILogger<ValidationExceptionHandler> logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        this.logger = logger;
    }

    // Maneja únicamente las excepciones de validación.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException ex)
        {
            return false;
        }

        logger.LogWarning(
            "Validación de Orders rechazada: {ErrorCode} - {ErrorMessage}",
            ex.ErrorCode,
            ex.Message);

        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            title = "Bad Request",
            status = 400,
            detail = "Los datos enviados son inválidos.",
            instance = context.Request.Path.Value,
            errorCode = ex.ErrorCode,
            errorMessage = ex.Message,
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}
