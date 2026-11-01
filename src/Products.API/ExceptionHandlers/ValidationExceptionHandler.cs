using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Ataja las ValidationException y devuelve un 400 con el formato de la consigna.
public class ValidationExceptionHandler : IExceptionHandler
{
    private ILogger<ValidationExceptionHandler> logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Si no es un ValidationException, no es para mí: que lo intente el siguiente handler
        if (exception is not ValidationException ex)
        {
            return false;
        }

        // Es un error de negocio esperado: se loguea como Warning, con su errorCode
        logger.LogWarning("Datos inválidos {ErrorCode}: {ErrorMessage}", ex.ErrorCode, ex.Message);

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