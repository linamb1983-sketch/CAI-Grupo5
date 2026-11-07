using Microsoft.AspNetCore.Diagnostics;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve un 500 sin exponer detalles técnicos.
public class GlobalExceptionHandler : IExceptionHandler
{
    // Maneja cualquier error que no haya sido procesado por otro handler.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "Internal Server Error",
            status = 500,
            detail = "Ocurrió un error inesperado.",
            instance = context.Request.Path.Value,
            errorCode = ErrorCodes.ORD_007,
            errorMessage = "Error interno al procesar la orden."
        }, cancellationToken);

        return true;
    }
}
