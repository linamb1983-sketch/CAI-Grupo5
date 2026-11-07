using Microsoft.AspNetCore.Diagnostics;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve el formato estándar para errores 404.
public class NotFoundExceptionHandler : IExceptionHandler
{
    private ILogger<NotFoundExceptionHandler> logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        this.logger = logger;
    }

    // Maneja únicamente las excepciones de recurso no encontrado.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException ex)
        {
            return false;
        }

        logger.LogWarning(
            "Recurso de Orders no encontrado: {ErrorCode} - {ErrorMessage}",
            ex.ErrorCode,
            ex.Message);

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
