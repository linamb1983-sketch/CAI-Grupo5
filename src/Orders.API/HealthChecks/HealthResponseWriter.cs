using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orders.API.HealthChecks;

// Arma la respuesta JSON de los health checks y filtra los chequeos por etiqueta.
public static class HealthResponseWriter
{
    // Selecciona solo los chequeos de vida del proceso.
    public static bool SoloLive(HealthCheckRegistration chequeo)
    {
        return chequeo.Tags.Contains("live");
    }

    // Selecciona solo los chequeos de disponibilidad.
    public static bool SoloReady(HealthCheckRegistration chequeo)
    {
        return chequeo.Tags.Contains("ready");
    }

    // Escribe el estado general y el detalle de cada chequeo.
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
            servicio = "Orders.API",
            duracionMs = reporte.TotalDuration.TotalMilliseconds,
            chequeos = chequeos
        });
    }
}
