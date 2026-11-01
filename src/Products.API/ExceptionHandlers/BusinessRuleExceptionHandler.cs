using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Ataja las BusinessRuleException y devuelve un 409 con el formato de la consigna.
// El "detail" cambia según el errorCode (PRD-003 y PRD-004 tienen textos distintos).
public class BusinessRuleExceptionHandler : IExceptionHandler
{
    private ILogger<BusinessRuleExceptionHandler> logger;

    public BusinessRuleExceptionHandler(ILogger<BusinessRuleExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Si no es un BusinessRuleException, no es para mí: que lo intente el siguiente handler
        if (exception is not BusinessRuleException ex)
        {
            return false;
        }

        // Es un error de negocio esperado: se loguea como Warning, con su errorCode
        logger.LogWarning("Regla de negocio {ErrorCode}: {ErrorMessage}", ex.ErrorCode, ex.Message);

        // Valores por defecto: 409 Conflict (PRD-003, producto duplicado)
        int status = 409;
        string title = "Conflict";
        string type = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
        string detail = "Ya existe un recurso con esos datos.";

        // PRD-004: producto con órdenes activas. Sigue siendo 409, pero cambia el detail
        if (ex.ErrorCode == ErrorCodes.PRD_004)
        {
            detail = "No se puede eliminar el recurso.";
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
            errorMessage = ex.Message,
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}