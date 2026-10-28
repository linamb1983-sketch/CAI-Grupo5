namespace Orders.API.Models;

// Representa un producto dentro de una orden.
public class OrderItem
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
