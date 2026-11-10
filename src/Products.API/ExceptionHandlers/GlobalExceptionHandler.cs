using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Red de seguridad: ataja cualquier error que no atajaron los anteriores.
// Devuelve un 500 con PRD-005. El nivel de detalle depende del entorno:
// en Development muestra el mensaje del error, en Production uno genérico.
// El stack trace nunca va en la respuesta, solo en el log.
public class GlobalExceptionHandler : IExceptionHandler
{
    private ILogger<GlobalExceptionHandler> logger;
    private IConfiguration configuracion;

    // Recibe el logger y la configuración (appsettings)
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IConfiguration configuracion)
    {
        this.logger = logger;
        this.configuracion = configuracion;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Es un error inesperado: se loguea como Error, con el detalle técnico completo
        logger.LogError(exception, "Error inesperado {ErrorCode}", ErrorCodes.PRD_005);

        // Por defecto (Production) se muestra un mensaje genérico
        string detalle = "Ocurrió un error inesperado.";

        // Lee la opción del appsettings como verdadero/falso
        bool mostrarDetalle = configuracion.GetValue<bool>("Errores:MostrarDetalle");

        // En Development se muestra el mensaje del error para ayudar a encontrarlo
        if (mostrarDetalle)
        {
            detalle = exception.Message;
        }

        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "Internal Server Error",
            status = 500,
            detail = detalle,
            instance = context.Request.Path.Value,
            errorCode = ErrorCodes.PRD_005,
            errorMessage = "Error interno al procesar el producto.",
            correlationId = $"{context.Items["CorrelationId"]}"
        }, cancellationToken);

        return true;
    }
}