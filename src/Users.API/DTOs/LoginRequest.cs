using System.ComponentModel.DataAnnotations;

namespace Users.API.DTOs;

/// <summary>
/// Datos que llegan del cliente para iniciar sesión (POST /api/users/login).
/// </summary>
public class LoginRequest
{
    /// <summary>Email con el que se registró el usuario.</summary>
    /// <example>maria@email.com</example>
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña del usuario.</summary>
    /// <example>MiPassword123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}