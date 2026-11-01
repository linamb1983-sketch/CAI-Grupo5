using Serilog.Context;

namespace Products.API.Middlewares;

// Le pone a cada pedido un número único (X-Correlation-Id) para poder seguirlo en los logs.
// Si el pedido ya trae uno (porque lo mandó otro servicio), usa ese mismo.
public class CorrelationIdMiddleware
{
    private const string NombreHeader = "X-Correlation-Id";

    private RequestDelegate siguiente;
    private ILogger<CorrelationIdMiddleware> logger;

    public CorrelationIdMiddleware(RequestDelegate siguiente, ILogger<CorrelationIdMiddleware> logger)
    {
        this.siguiente = siguiente;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Si el pedido trae un Correlation ID lo usamos, si no creamos uno nuevo
        string correlationId = context.Request.Headers[NombreHeader].ToString();

        if (correlationId == "")
        {
            correlationId = Guid.NewGuid().ToString();
        }

        // Lo guardamos para que lo lean los ExceptionHandlers
        context.Items["CorrelationId"] = correlationId;

        // Lo devolvemos en el header de la respuesta (se pone justo antes de enviarla)
        context.Response.OnStarting(PonerHeaderEnRespuesta, context);

        string endpoint = $"{context.Request.Method} {context.Request.Path}";

        // Todo lo que se loguee dentro de este bloque lleva CorrelationId y Endpoint
        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("Endpoint", endpoint))
        {
            logger.LogInformation("Inicio del request");

            await siguiente(context);
        }
    }

    // Agrega el X-Correlation-Id a la respuesta
    private Task PonerHeaderEnRespuesta(object estado)
    {
        HttpContext context = (HttpContext)estado;
        context.Response.Headers[NombreHeader] = $"{context.Items["CorrelationId"]}";
        return Task.CompletedTask;
    }
}