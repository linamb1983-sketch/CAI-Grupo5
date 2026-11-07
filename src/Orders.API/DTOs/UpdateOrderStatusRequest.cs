using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Estado al que se intenta cambiar una orden.</summary>
public class UpdateOrderStatusRequest
{
    /// <summary>Nuevo estado.</summary>
    /// <example>Confirmada</example>
    [Required(ErrorMessage = "El estado es obligatorio")]
    public string Estado { get; set; } = string.Empty;
}
