using Notifications.API.Models;

namespace Notifications.API.Repositories;

// Guarda las notificaciones en una lista en memoria.
// Cuando llegue la librería de la cátedra, cambiamos solo esta clase.
public class NotificationRepository
{
    // La lista es privada: solo se modifica con los métodos de esta clase
    private List<Notification> notificaciones = new List<Notification>();

    // Agrega una notificación a la lista
    public void Agregar(Notification notificacion)
    {
        notificaciones.Add(notificacion);
    }

    // Devuelve las notificaciones de un usuario.
    // Si no tiene ninguna, devuelve una lista vacía.
    public List<Notification> ObtenerPorUsuario(Guid usuarioId)
    {
        List<Notification> resultado = new List<Notification>();

        foreach (Notification n in notificaciones)
        {
            if (n.UsuarioId == usuarioId)
            {
                resultado.Add(n);
            }
        }

        return resultado;
    }
}