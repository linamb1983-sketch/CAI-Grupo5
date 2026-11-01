using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Red de seguridad: ataja cualquier error que no atajaron los anteriores.
// Devuelve un 500 con PRD-005 y NO muestra el detalle técnico del error.
public class GlobalExceptionHandler : IExceptionHandler
{
    private ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Es un error inesperado: se loguea como Error, con el detalle técnico (solo en el log, nunca en la respuesta)
        logger.LogError(exception, "Error inesperado {ErrorCode}", ErrorCodes.PRD_005);

        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "Internal Server Error",
            status = 500,
            detail = "Ocurrió un error inesperado.",
            instance = context.Request.Path.Value,
            errorCode = ErrorCodes.PRD_005,
            errorMessage = "Error interno al procesar el producto.",
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}