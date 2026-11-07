using Microsoft.AspNetCore.Diagnostics;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve un 500 sin exponer detalles técnicos.
public class GlobalExceptionHandler : IExceptionHandler
{
    private ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    // Maneja cualquier error que no haya sido procesado por otro handler.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error inesperado {ErrorCode}", ErrorCodes.ORD_007);

        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "Internal Server Error",
            status = 500,
            detail = "Ocurrió un error inesperado.",
            instance = context.Request.Path.Value,
            errorCode = ErrorCodes.ORD_007,
            errorMessage = "Error interno al procesar la orden.",
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}
