using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Datos de un producto que se solicita en una orden.</summary>
public class OrderItemRequest
{
    /// <summary>Id del producto.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad solicitada; debe ser mayor a cero.</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
    public int Cantidad { get; set; }
}
