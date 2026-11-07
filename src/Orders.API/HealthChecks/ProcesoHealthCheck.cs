using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orders.API.HealthChecks;

// Indica que el proceso de Orders.API está en ejecución.
public class ProcesoHealthCheck : IHealthCheck
{
    // Devuelve Healthy mientras la API puede atender este chequeo.
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("El servicio Orders.API está en ejecución."));
    }
}
