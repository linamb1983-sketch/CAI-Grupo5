using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

// Estado al que se intenta cambiar una orden.
public class UpdateOrderStatusRequest
{
    [Required(ErrorMessage = "El estado es obligatorio")]
    public string Estado { get; set; } = string.Empty;
}
