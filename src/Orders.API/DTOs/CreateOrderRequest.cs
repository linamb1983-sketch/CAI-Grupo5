using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Datos necesarios para crear una orden.</summary>
public class CreateOrderRequest
{
    /// <summary>Id del usuario que realiza la orden.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid UsuarioId { get; set; }

    /// <summary>Productos y cantidades incluidas en la orden.</summary>
    /// <example>[{"productoId":"3fa85f64-5717-4562-b3fc-2c963f66afa6","cantidad":2}]</example>
    [Required(ErrorMessage = "La lista de productos es obligatoria")]
    [MinLength(1, ErrorMessage = "La orden debe incluir al menos un producto")]
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
}
