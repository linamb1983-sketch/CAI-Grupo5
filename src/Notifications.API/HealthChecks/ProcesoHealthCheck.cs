using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Notifications.API.HealthChecks;

// Chequeo "live" (está vivo): si la API puede responder este pedido, el proceso está vivo.
// No revisa nada más. Sirve para saber si hay que reiniciar el servicio.
public class ProcesoHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("El servicio Notifications.API está en ejecución."));
    }
}