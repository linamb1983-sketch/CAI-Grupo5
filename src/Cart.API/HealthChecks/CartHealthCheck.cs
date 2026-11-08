using Microsoft.Extensions.Diagnostics.HealthChecks;
using Cart.API.Repositories;

namespace Cart.API.HealthChecks;

// Chequeo "ready" (está listo): revisa que el repositorio de carritos responda.
// Cuando llegue la librería de la cátedra, acá se chequea que la persistencia esté disponible.
public class CartHealthCheck : IHealthCheck
{
    private CartRepository repositorio;

    // El repositorio llega por inyección de dependencias (igual que en el Service)
    public CartHealthCheck(CartRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            int cantidad = repositorio.Cantidad();
            return Task.FromResult(HealthCheckResult.Healthy($"El repositorio responde ({cantidad} carritos)."));
        }
        catch (Exception ex)
        {
            // Si el repositorio falla, el servicio no está listo para recibir pedidos
            return Task.FromResult(HealthCheckResult.Unhealthy("El repositorio de carritos no responde.", ex));
        }
    }
}