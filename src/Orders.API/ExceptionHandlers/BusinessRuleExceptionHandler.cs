using Microsoft.AspNetCore.Diagnostics;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve 422 para stock insuficiente y 409 para otras reglas de negocio.
public class BusinessRuleExceptionHandler : IExceptionHandler
{
    // Maneja las reglas de negocio con el estado correspondiente al código.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BusinessRuleException ex)
        {
            return false;
        }

        int status = 409;
        string title = "Conflict";
        string type = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
        string detail = "La operación no puede completarse por una regla del negocio.";

        if (ex.ErrorCode == ErrorCodes.ORD_005)
        {
            status = 422;
            title = "Unprocessable Entity";
            type = "https://tools.ietf.org/html/rfc4918#section-11.2";
            detail = "No se puede procesar la solicitud.";
        }
        else if (ex.ErrorCode == ErrorCodes.ORD_006)
        {
            detail = "No se puede modificar el estado.";
        }

        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(new
        {
            type,
            title,
            status,
            detail,
            instance = context.Request.Path.Value,
            errorCode = ex.ErrorCode,
            errorMessage = ex.Message
        }, cancellationToken);

        return true;
    }
}
