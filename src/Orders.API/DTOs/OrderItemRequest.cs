using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

// Datos de un producto que se solicita en una orden.
public class OrderItemRequest
{
    public Guid ProductoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
    public int Cantidad { get; set; }
}
