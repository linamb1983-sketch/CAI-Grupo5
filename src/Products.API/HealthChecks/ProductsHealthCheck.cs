using Microsoft.Extensions.Diagnostics.HealthChecks;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.HealthChecks;

// Chequeo "ready" (está listo): revisa que el repositorio de productos responda.
// Cuando llegue la librería de la cátedra, acá se chequea que la persistencia esté disponible.
public class ProductsHealthCheck : IHealthCheck
{
    private ProductRepository repositorio;

    // El repositorio llega por inyección de dependencias (igual que en el Service)
    public ProductsHealthCheck(ProductRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            List<Product> productos = repositorio.ObtenerTodos();
            return Task.FromResult(HealthCheckResult.Healthy($"El repositorio responde ({productos.Count} productos)."));
        }
        catch (Exception ex)
        {
            // Si el repositorio falla, el servicio no está listo para recibir pedidos
            return Task.FromResult(HealthCheckResult.Unhealthy("El repositorio de productos no responde.", ex));
        }
    }
}