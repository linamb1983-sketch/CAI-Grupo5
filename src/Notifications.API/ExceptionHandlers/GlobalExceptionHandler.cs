using Microsoft.AspNetCore.Diagnostics;
using Notifications.API.Exceptions;

namespace Notifications.API.ExceptionHandlers;

// Red de seguridad: ataja cualquier error que no atajaron los anteriores.
// Devuelve un 500 con NTF-004 y NO muestra el detalle técnico del error.
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
            errorCode = ErrorCodes.NTF_004,
            errorMessage = "Error interno al procesar la notificación."
        }, cancellationToken);

        return true;
    }
}