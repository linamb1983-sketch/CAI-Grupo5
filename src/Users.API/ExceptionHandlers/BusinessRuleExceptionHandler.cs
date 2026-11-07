using Microsoft.AspNetCore.Diagnostics;
using Users.API.Exceptions;

namespace Users.API.ExceptionHandlers;

// Ataja las BusinessRuleException y devuelve el status que corresponde al errorCode:
// USR-001 -> 409, USR-003 -> 401, USR-004 y USR-005 -> 403.
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

        // Valores por defecto: 409 Conflict (USR-001, email ya registrado)
        int status = 409;
        string title = "Conflict";
        string type = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
        string detail = "Ya existe un recurso con esos datos.";

        // USR-003: credenciales incorrectas -> 401
        if (ex.ErrorCode == ErrorCodes.USR_003)
        {
            status = 401;
            title = "Unauthorized";
            type = "https://tools.ietf.org/html/rfc7235#section-3.1";
            detail = "Las credenciales no son válidas.";
        }
        // USR-004 y USR-005: usuario bloqueado -> 403
        else if (ex.ErrorCode == ErrorCodes.USR_004 || ex.ErrorCode == ErrorCodes.USR_005)
        {
            status = 403;
            title = "Forbidden";
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.3";
            detail = "El acceso está prohibido.";
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