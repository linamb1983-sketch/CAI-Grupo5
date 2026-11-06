using Microsoft.AspNetCore.Diagnostics;
using Cart.API.Exceptions;

namespace Cart.API.ExceptionHandlers;

// Red de seguridad: ataja cualquier error que no atajaron los anteriores.
// Devuelve un 500 con CRT-005 y NO muestra el detalle técnico del error.
public class GlobalExceptionHandler : IExceptionHandler
{
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
            errorCode = ErrorCodes.CRT_005,
            errorMessage = "Error interno al procesar el carrito."
        }, cancellationToken);

        return true;
    }
}