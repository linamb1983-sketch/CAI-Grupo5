using Microsoft.AspNetCore.Diagnostics;
using Cart.API.Exceptions;

namespace Cart.API.ExceptionHandlers;

// Ataja las BusinessRuleException (reglas del negocio).
// Para CRT-003 (stock insuficiente) devuelve 422, como pide la consigna. Para cualquier otra, 409.
public class BusinessRuleExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Si no es un BusinessRuleException, no es para mí: que lo intente el siguiente handler
        if (exception is not BusinessRuleException ex)
        {
            return false;
        }

        // Valores por defecto: 409 Conflict
        int status = 409;
        string title = "Conflict";
        string type = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
        string detail = "La operación no puede completarse por una regla del negocio.";

        // CRT-003: stock insuficiente. La consigna pide 422
        if (ex.ErrorCode == ErrorCodes.CRT_003)
        {
            status = 422;
            title = "Unprocessable Entity";
            type = "https://tools.ietf.org/html/rfc4918#section-11.2";
            detail = "No se puede procesar la solicitud.";
        }

        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(new
        {
            type = type,
            title = title,
            status = status,
            detail = detail,
            instance = context.Request.Path.Value,
            errorCode = ex.ErrorCode,
            errorMessage = ex.Message
        }, cancellationToken);

        return true;
    }
}