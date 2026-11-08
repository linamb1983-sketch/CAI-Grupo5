using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Notifications.API.HealthChecks;

// Arma la respuesta JSON de los health checks (Healthy, Degraded o Unhealthy).
// Tiene también los dos filtros que dicen qué chequeos corre cada endpoint.
public static class HealthResponseWriter
{
    // Filtro de /health/live: solo los chequeos con la etiqueta "live"
    public static bool SoloLive(HealthCheckRegistration chequeo)
    {
        return chequeo.Tags.Contains("live");
    }

    // Filtro de /health/ready: solo los chequeos con la etiqueta "ready"
    public static bool SoloReady(HealthCheckRegistration chequeo)
    {
        return chequeo.Tags.Contains("ready");
    }

    // Escribe el resultado en formato JSON
    public static async Task Escribir(HttpContext context, HealthReport reporte)
    {
        List<object> chequeos = new List<object>();

        foreach (KeyValuePair<string, HealthReportEntry> item in reporte.Entries)
        {
            chequeos.Add(new
            {
                nombre = item.Key,
                estado = item.Value.Status.ToString(),
                descripcion = item.Value.Description
            });
        }

        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsJsonAsync(new
        {
            estado = reporte.Status.ToString(),
            servicio = "Notifications.API",
            duracionMs = reporte.TotalDuration.TotalMilliseconds,
            chequeos = chequeos
        });
    }
}