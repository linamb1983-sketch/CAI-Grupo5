using Microsoft.Extensions.Diagnostics.HealthChecks;
using Users.API.Repositories;

namespace Users.API.HealthChecks;

// Chequeo "ready" (está listo): revisa que el repositorio de usuarios responda.
// Cuando llegue la librería de la cátedra, acá se chequea que la persistencia esté disponible.
public class UsersHealthCheck : IHealthCheck
{
    private UserRepository repositorio;

    // El repositorio llega por inyección de dependencias (igual que en el Service)
    public UsersHealthCheck(UserRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            int cantidad = repositorio.Cantidad();
            return Task.FromResult(HealthCheckResult.Healthy($"El repositorio responde ({cantidad} usuarios)."));
        }
        catch (Exception ex)
        {
            // Si el repositorio falla, el servicio no está listo para recibir pedidos
            return Task.FromResult(HealthCheckResult.Unhealthy("El repositorio de usuarios no responde.", ex));
        }
    }
}