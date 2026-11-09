using Microsoft.Extensions.Diagnostics.HealthChecks;
using Notifications.API.Repositories;

namespace Notifications.API.HealthChecks;

// Chequeo "ready" (está listo): revisa que el repositorio de notificaciones responda.
// Cuando llegue la librería de la cátedra, acá se chequea que la persistencia esté disponible.
public class NotificationsHealthCheck : IHealthCheck
{
    private NotificationRepository repositorio;

    // El repositorio llega por inyección de dependencias (igual que en el Service)
    public NotificationsHealthCheck(NotificationRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            int cantidad = repositorio.Cantidad();
            return Task.FromResult(HealthCheckResult.Healthy($"El repositorio responde ({cantidad} notificaciones)."));
        }
        catch (Exception ex)
        {
            // Si el repositorio falla, el servicio no está listo para recibir pedidos
            return Task.FromResult(HealthCheckResult.Unhealthy("El repositorio de notificaciones no responde.", ex));
        }
    }
}