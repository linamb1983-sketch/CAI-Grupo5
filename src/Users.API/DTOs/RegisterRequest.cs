using System.ComponentModel.DataAnnotations;

namespace Users.API.DTOs;

/// <summary>
/// Datos que llegan del cliente para registrar un usuario (POST /api/users/register).
/// No tiene Id, FechaRegistro, Activo ni IntentosFallidos porque eso lo genera el servidor.
/// </summary>
public class RegisterRequest
{
    /// <summary>Nombre del usuario. Obligatorio.</summary>
    /// <example>María</example>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellido del usuario. Obligatorio.</summary>
    /// <example>González</example>
    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>Email del usuario. Obligatorio, con formato válido y único.</summary>
    /// <example>maria@email.com</example>
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña del usuario. Obligatoria. Se guarda como hash, nunca se devuelve.</summary>
    /// <example>MiPassword123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}