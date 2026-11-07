using Serilog.Context;

namespace Orders.API.Middlewares;

// Reutiliza o genera un identificador para seguir cada solicitud entre servicios.
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
        string correlationId = context.Request.Headers[NombreHeader].ToString();

        if (correlationId == "")
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Items["CorrelationId"] = correlationId;
        context.Response.OnStarting(PonerHeaderEnRespuesta, context);

        string endpoint = $"{context.Request.Method} {context.Request.Path}";

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("Endpoint", endpoint))
        {
            logger.LogInformation("Inicio del request");
            await siguiente(context);
        }
    }

    private Task PonerHeaderEnRespuesta(object estado)
    {
        HttpContext context = (HttpContext)estado;
        context.Response.Headers[NombreHeader] = $"{context.Items["CorrelationId"]}";
        return Task.CompletedTask;
    }
}
