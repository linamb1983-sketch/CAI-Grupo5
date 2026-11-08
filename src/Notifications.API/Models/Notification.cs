namespace Notifications.API.Models;

/// <summary>
/// Notificación enviada (simulada) a un usuario. Es lo que guardamos en el repositorio.
/// </summary>
public class Notification
{
    /// <summary>Identificador único de la notificación.</summary>
    public Guid Id { get; set; }

    /// <summary>Usuario destinatario (tiene que existir en Users API).</summary>
    public Guid UsuarioId { get; set; }

    /// <summary>Texto de la notificación. Obligatorio, máximo 500 caracteres.</summary>
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>Canal: Email, Push o SMS.</summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>Estado del envío: Pendiente, Enviada o Fallida.</summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>Fecha en que se registró. La asigna el servidor.</summary>
    public DateTime FechaEnvio { get; set; }
}