using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Ataja las NotFoundException y devuelve un 404 con el formato de la consigna.
public class NotFoundExceptionHandler : IExceptionHandler
{
    private ILogger<NotFoundExceptionHandler> logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Si no es un NotFoundException, no es para mí: que lo intente el siguiente handler
        if (exception is not NotFoundException ex)
        {
            return false;
        }

        // Es un error de negocio esperado: se loguea como Warning, con su errorCode
        logger.LogWarning("Recurso no encontrado {ErrorCode}: {ErrorMessage}", ex.ErrorCode, ex.Message);

        context.Response.StatusCode = 404;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            title = "Not Found",
            status = 404,
            detail = "El recurso solicitado no fue encontrado.",
            instance = context.Request.Path.Value,
            errorCode = ex.ErrorCode,
            errorMessage = ex.Message,
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}