using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orders.API.Models;
using Orders.API.Repositories;

namespace Orders.API.HealthChecks;

// Verifica que el repositorio de órdenes pueda responder.
public class OrdersHealthCheck : IHealthCheck
{
    private OrderRepository repositorio;

    // Recibe el repositorio de órdenes por inyección de dependencias.
    public OrdersHealthCheck(OrderRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    // Devuelve Healthy si se puede consultar el repositorio.
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            List<Order> ordenes = repositorio.ObtenerTodas();
            return Task.FromResult(HealthCheckResult.Healthy($"El repositorio responde ({ordenes.Count} órdenes)."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("El repositorio de órdenes no responde.", ex));
        }
    }
}
