namespace Users.API.Models;

/// <summary>
/// Usuario del sistema (Apéndice A de la consigna).
/// </summary>
public class User
{
    /// <summary>Identificador único del usuario.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre del usuario. Obligatorio.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellido del usuario. Obligatorio.</summary>
    public string Apellido { get; set; } = string.Empty;

    /// <summary>Email del usuario. Obligatorio, único y con formato válido.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Hash de la contraseña. Nunca se devuelve en ninguna respuesta.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Fecha en que se registró. La asigna el servidor.</summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>false cuando el usuario está bloqueado.</summary>
    public bool Activo { get; set; }

    /// <summary>Login fallidos seguidos. Se resetea al loguearse bien.</summary>
    public int IntentosFallidos { get; set; }
}