namespace Orders.API.DTOs;

/// <summary>Datos de un producto incluido en la respuesta de una orden.</summary>
public class OrderItemResponse
{
    /// <summary>Id del producto.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }
    /// <summary>Cantidad incluida.</summary>
    /// <example>2</example>
    public int Cantidad { get; set; }
    /// <summary>Precio unitario al crear la orden.</summary>
    /// <example>99.99</example>
    public decimal PrecioUnitario { get; set; }
}
