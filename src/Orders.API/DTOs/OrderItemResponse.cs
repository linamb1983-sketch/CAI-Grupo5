namespace Orders.API.DTOs;

// Datos de un producto incluido en la respuesta de una orden.
public class OrderItemResponse
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
