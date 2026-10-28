using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

// Datos necesarios para crear una orden.
public class CreateOrderRequest
{
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "La lista de productos es obligatoria")]
    [MinLength(1, ErrorMessage = "La orden debe incluir al menos un producto")]
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
}
