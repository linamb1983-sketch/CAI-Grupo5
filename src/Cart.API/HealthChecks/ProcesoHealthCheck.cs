using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cart.API.HealthChecks;

// Chequeo "live" : si la API puede responder este pedido, el proceso está vivo.
// No revisa nada más. Sirve para saber si hay que reiniciar el servicio.
public class ProcesoHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("El servicio Cart.API está en ejecución."));
    }
}