using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Orders.API.Exceptions;

namespace Orders.API.ExceptionHandlers;

// Devuelve un 500 sin exponer detalles técnicos.
public class GlobalExceptionHandler : IExceptionHandler
{
    private ILogger<GlobalExceptionHandler> logger;
    private IConfiguration configuracion;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IConfiguration configuracion)
    {
        this.logger = logger;
        this.configuracion = configuracion;
    }

    // Maneja errores inesperados y muestra detalle solo si la configuración lo permite.
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error inesperado {ErrorCode}", ErrorCodes.ORD_007);

        context.Response.StatusCode = 500;

        string detail = "Ocurrió un error inesperado.";
        bool mostrarDetalle = configuracion.GetValue<bool>("Errores:MostrarDetalle");

        if (mostrarDetalle)
        {
            detail = exception.Message;
        }

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "Internal Server Error",
            status = 500,
            detail,
            instance = context.Request.Path.Value,
            errorCode = ErrorCodes.ORD_007,
            errorMessage = "Error interno al procesar la orden.",
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}
